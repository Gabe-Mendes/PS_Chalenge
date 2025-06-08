using Questao5.Infrastructure.Database.Repository;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Questao5.Infrastructure.Services.Middleware
{
    public class IdempotenciaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IdempotenciaMiddleware> _logger;

        public IdempotenciaMiddleware(RequestDelegate next, ILogger<IdempotenciaMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IIdempotenciaRepository idempotenciaRepository)
        {
            // Verificar se é uma requisição POST para o endpoint de movimentação
            if (context.Request.Method == "POST" && context.Request.Path.StartsWithSegments("/api/movimento"))
            {
                // Verificar se a chave de idempotência está presente
                if (context.Request.Headers.TryGetValue("X-Chave-Idempotencia", out var chaveIdempotencia) && !string.IsNullOrEmpty(chaveIdempotencia))
                {
                    try
                    {
                        // Verificar se a requisição já foi processada
                        var existeChave = await idempotenciaRepository.ExisteChaveIdempotencia(chaveIdempotencia);
                        if (existeChave)
                        {
                            // Recuperar o resultado da requisição anterior
                            var resultadoAnterior = await idempotenciaRepository.ObterResultadoIdempotencia(chaveIdempotencia);
                            if (!string.IsNullOrEmpty(resultadoAnterior))
                            {
                                // Retornar o resultado anterior
                                context.Response.ContentType = "application/json";
                                context.Response.StatusCode = 200;
                                await context.Response.WriteAsync(resultadoAnterior);
                                _logger.LogInformation("Requisição idempotente recuperada: {ChaveIdempotencia}", chaveIdempotencia);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao verificar idempotência: {ChaveIdempotencia}", chaveIdempotencia);
                        // Continuar com o pipeline em caso de erro na verificação
                    }
                }
            }

            // Continuar com o pipeline
            await _next(context);
        }
    }
}