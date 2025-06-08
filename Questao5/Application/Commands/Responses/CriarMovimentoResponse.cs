namespace Questao5.Application.Commands.Responses
{
    public class CriarMovimentoResponse
    {
        public string? IdMovimento { get; set; }
        public bool Sucesso { get; set; }
        public string? MensagemErro { get; set; }
    }
}