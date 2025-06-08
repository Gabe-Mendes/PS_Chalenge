using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database.Repository
{
    public interface IMovimentoRepository
    {
        Task<ObterMovimentosContaCorrenteResponse> ObterMovimentos(ObterMovimentosContaCorrenteRequest request);
        Task<InserirMovimentoResponse> InserirMovimento(InserirMovimentoRequest request);
    }
}