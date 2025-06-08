using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class InserirMovimentoRequest
    {
        public string? IdMovimento { get; set; }
        public string? IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public TipoMovimento TipoMovimento { get; set; }        
        public decimal Valor { get; private set; }

        public InserirMovimentoRequest(string? idMovimento, string? idContaCorrente, DateTime dataMovimento, TipoMovimento tipoMovimento, decimal valor)
        {
            IdMovimento = idMovimento;
            IdContaCorrente = idContaCorrente;
            DataMovimento = dataMovimento;
            TipoMovimento = tipoMovimento;
            Valor = valor;            
        }
    }
}