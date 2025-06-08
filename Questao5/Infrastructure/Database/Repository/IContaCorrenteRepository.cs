using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database.Repository
{
    public interface IContaCorrenteRepository
    {
        Task<ObterContaCorrenteResponse> ObterContaCorrente(ObterContaCorrenteRequest request);        
    }
}