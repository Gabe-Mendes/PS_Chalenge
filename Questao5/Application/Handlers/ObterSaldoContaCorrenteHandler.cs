using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Application.Handlers
{
    public class ObterSaldoContaCorrenteHandler : IRequestHandler<ObterSaldoContaCorrenteRequest, ObterSaldoContaCorrenteResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public ObterSaldoContaCorrenteHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<ObterSaldoContaCorrenteResponse> Handle(ObterSaldoContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            var response = new ObterSaldoContaCorrenteResponse
            {
                DataConsulta = DateTime.Now
            };

            // Busca a conta corrente
            var contaCorrenteResponse = await _contaCorrenteRepository.ObterContaCorrente(new ObterContaCorrenteRequest { IdContaCorrente = request.IdContaCorrente })!;

            // Validações da conta corrente
            if (!contaCorrenteResponse.Sucesso || contaCorrenteResponse.ContaCorrente == null)
            {
                response.Sucesso = false;
                response.MensagemErro = MensagemErro.INVALID_ACCOUNT;
                return response;
            }

            if (contaCorrenteResponse.ContaCorrente.Ativo == 0)
            {
                response.Sucesso = false;
                response.MensagemErro = MensagemErro.INACTIVE_ACCOUNT;
                return response;
            }

            // Busca os movimentos da conta corrente
            var movimentosResponse = await _movimentoRepository.ObterMovimentos(new ObterMovimentosContaCorrenteRequest { IdContaCorrente = request.IdContaCorrente });

            // Validações dos movimentos
            if (!movimentosResponse.Sucesso)
            {
                response.Sucesso = false;
                response.MensagemErro = movimentosResponse.MensagemErro;
                return response;
            }

            // Calculo de Saldo
            decimal saldo = 0;

            foreach (var movimento in movimentosResponse.Movimentos)
            {
                if (movimento.TipoMovimento == "C")
                {
                    saldo += movimento.Valor;
                }
                else if (movimento.TipoMovimento == "D")
                {
                    saldo -= movimento.Valor;
                }
            }

            response.NumeroConta = contaCorrenteResponse.ContaCorrente.Numero;
            response.NomeTitular = contaCorrenteResponse.ContaCorrente.Nome;
            response.Saldo = saldo;
            response.Sucesso = true;

            return response;
        }
    }
}