using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repository
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig _dbConfig;

        public MovimentoRepository(DatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public async Task<ObterMovimentosContaCorrenteResponse> ObterMovimentos(ObterMovimentosContaCorrenteRequest request)
        {
            var response = new ObterMovimentosContaCorrenteResponse();

            using (var connection = new SqliteConnection(_dbConfig.Name))
            {
                await connection.OpenAsync();

                var querySql = @"
                SELECT idmovimento as IdMovimento
                     , idcontacorrente as IdContaCorrente
                     , datamovimento as DataMovimento
                     , tipomovimento as TipoMovimento
                     , valor as Valor 
                  FROM movimento 
                 WHERE idcontacorrente = @IdContaCorrente
                ";

                var movimentos = await connection
                    .QueryAsync<Movimento>(querySql, new { IdContaCorrente = request.IdContaCorrente });

                response.Movimentos = movimentos.AsList();
                response.Sucesso = true;
            }
            return response;
        }
        
        public async Task<InserirMovimentoResponse> InserirMovimento(InserirMovimentoRequest request)
        {
            var response = new InserirMovimentoResponse();

            using (var connection = new SqliteConnection(_dbConfig.Name))
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                           VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

                var result = await connection.ExecuteAsync(
                    sql,
                    new { 
                        IdMovimento = request.IdMovimento,
                        IdContaCorrente = request.IdContaCorrente,
                        DataMovimento = request.DataMovimento.ToString("yyyy-MM-dd HH:mm:ss"),
                        TipoMovimento = ((char)request.TipoMovimento).ToString(),
                        Valor = request.Valor
                    }
                );

                response.Sucesso = result > 0;
            }

            return response;
        }
    }
}