using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repository
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig _dbConfig;

        public IdempotenciaRepository(DatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public async Task<bool> ExisteChaveIdempotencia(string chaveIdempotencia)
        {
            using (var connection = new SqliteConnection(_dbConfig.Name))
            {
                await connection.OpenAsync();

                var sql = "SELECT COUNT(1) FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
                var count = await connection.ExecuteScalarAsync<int>(sql, new { ChaveIdempotencia = chaveIdempotencia });

                return count > 0;
            }
        }

        public async Task<string> ObterResultadoIdempotencia(string chaveIdempotencia)
        {
            using (var connection = new SqliteConnection(_dbConfig.Name))
            {
                await connection.OpenAsync();

                var sql = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
                return await connection.QueryFirstOrDefaultAsync<string>(sql, new { ChaveIdempotencia = chaveIdempotencia });
            }
        }

        public async Task SalvarIdempotencia(string chaveIdempotencia, string requisicao, string resultado)
        {
            using (var connection = new SqliteConnection(_dbConfig.Name))
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) 
                           VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

                await connection.ExecuteAsync(sql, new
                {
                    ChaveIdempotencia = chaveIdempotencia,
                    Requisicao = requisicao,
                    Resultado = resultado
                });
            }
        }
    }
}