namespace Questao5.Application.Queries.Responses
{
    public class ObterSaldoContaCorrenteResponse
    {
        public int NumeroConta { get; set; }
        public string? NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public decimal Saldo { get; set; }
        public bool Sucesso { get; set; }
        public string? MensagemErro { get; set; }        
    }
}