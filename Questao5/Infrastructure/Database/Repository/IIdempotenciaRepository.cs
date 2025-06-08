namespace Questao5.Infrastructure.Database.Repository
{
    public interface IIdempotenciaRepository
    {
        Task<bool> ExisteChaveIdempotencia(string chaveIdempotencia);
        Task<string> ObterResultadoIdempotencia(string chaveIdempotencia);
        Task SalvarIdempotencia(string chaveIdempotencia, string requisicao, string resultado);
    }
}