using Microsoft.AspNetCore.Http;
using Questao5.Infrastructure.Database.Repository;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Services.Middleware
{
    public class ResponseCapturaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseCapturaMiddleware> _logger;

        public ResponseCapturaMiddleware(RequestDelegate next, ILogger<ResponseCapturaMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context, IIdempotenciaRepository idempotenciaRepository)
        {
            // Verificar se é uma requisição POST para o endpoint de movimentação
            if (context.Request.Method == "POST" && context.Request.Path.StartsWithSegments("/api/movimento"))
            {
                // Verificar se a chave de idempotência está presente
                if (context.Request.Headers.TryGetValue("X-Chave-Idempotencia", out var idempotencyKey) && !string.IsNullOrEmpty(idempotencyKey))
                {
                    // Verificar se a chave já existe (para evitar reprocessamento)
                    var existeChave = await idempotenciaRepository.ExisteChaveIdempotencia(idempotencyKey);
                    if (existeChave)
                    {
                        // Se a chave já existe, o IdempotenciaMiddleware já deve ter tratado
                        // Apenas continuamos o pipeline
                        await _next(context);
                        return;
                    }

                    // Capturar o corpo da requisição
                    var requestBody = await CaptureRequestBody(context.Request);

                    // Substituir o response stream por um que podemos ler
                    var originalBodyStream = context.Response.Body;
                    using var responseBody = new MemoryStream();
                    context.Response.Body = responseBody;

                    // Continuar com o pipeline
                    await _next(context);

                    // Capturar a resposta
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();

                    // Salvar a idempotência para qualquer resposta (sucesso ou erro)
                    // Isso permite que erros também sejam idempotentes
                    try
                    {
                        await idempotenciaRepository.SalvarIdempotencia(
                            idempotencyKey.ToString(),
                            requestBody,
                            responseBodyText
                        );
                        _logger.LogInformation("Idempotência salva com sucesso: {ChaveIdempotencia}", idempotencyKey);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao salvar idempotência para a chave: {ChaveIdempotencia}", idempotencyKey);
                    }

                    // Copiar a resposta de volta para o stream original
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                    return;
                }
            }

            // Continuar com o pipeline para requisições que não precisam de idempotência
            await _next(context);
        }

        private async Task<string> CaptureRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            
            var body = await reader.ReadToEndAsync();
            
            // Resetar a posição do stream para que possa ser lido novamente
            request.Body.Seek(0, SeekOrigin.Begin);
            
            return body;
        }
    }
}