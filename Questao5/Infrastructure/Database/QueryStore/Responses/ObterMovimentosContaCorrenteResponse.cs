using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class ObterMovimentosContaCorrenteResponse
    {
        public List<Movimento>? Movimentos { get; set; }
        public bool Sucesso { get; set; }
        public string? MensagemErro { get; set; }        
    }
}