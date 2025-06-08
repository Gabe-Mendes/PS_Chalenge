using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.Repository;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Handlers
{
    public class CriarMovimentoHandler : IRequestHandler<CriarMovimentoRequest, CriarMovimentoResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly ILogger<CriarMovimentoHandler> _logger;

        public CriarMovimentoHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository, IIdempotenciaRepository idempotenciaRepository, ILogger<CriarMovimentoHandler> logger)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
            _idempotenciaRepository = idempotenciaRepository;
            _logger = logger;
        }

        public async Task<CriarMovimentoResponse> Handle(CriarMovimentoRequest request, CancellationToken cancellationToken)
        {
            // A verificação de idempotência foi removida daqui pois já é feita pelo IdempotenciaMiddleware

            var response = new CriarMovimentoResponse();

            //Validações da conta corrente
            var contaCorrente = await _contaCorrenteRepository.ObterContaCorrente(new ObterContaCorrenteRequest { IdContaCorrente = request.IdContaCorrente });
            if (!contaCorrente.Sucesso || contaCorrente.ContaCorrente == null)
            {
                response.Sucesso = false;
                response.MensagemErro = MensagemErro.INVALID_ACCOUNT;
                return response;
            }

            if (contaCorrente.ContaCorrente.Ativo == 0)
            {
                response.Sucesso = false;
                response.MensagemErro = MensagemErro.INACTIVE_ACCOUNT;
                return response;
            }

            // Validar tipo de movimento
            if (request.TipoMovimento != TipoMovimento.Credito && 
                request.TipoMovimento != TipoMovimento.Debito)
            {
                response.Sucesso = false;
                response.MensagemErro = MensagemErro.INVALID_TYPE;
                return response;
            }

            // Validar valor do movimento
            if (request.Valor <= 0)
            {
                response.Sucesso = false;
                response.MensagemErro = MensagemErro.INVALID_VALUE;
                return response;
            }

            // Criar o movimento da Conta Corrente
            var idMovimento = Guid.NewGuid().ToString();

            var inserirMovimentoRequest =
            new InserirMovimentoRequest(
                idMovimento: idMovimento,
                idContaCorrente: request.IdContaCorrente,
                dataMovimento: DateTime.Now,
                tipoMovimento: request.TipoMovimento,
                valor: request.Valor
                );

            //Inserir o movimento na base de dados
            var movimento = await _movimentoRepository.InserirMovimento(inserirMovimentoRequest);

            if (!movimento.Sucesso)
            {
                response.Sucesso = false;
                response.MensagemErro = MensagemErro.ERROR_MOVEMENT;
                return response;
            }

            response.IdMovimento = idMovimento;
            response.Sucesso = true;
            
            return response;
        }
    }
}