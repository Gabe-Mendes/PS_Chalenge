using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig _dbConfig;

        public ContaCorrenteRepository(DatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }
        public async Task<ObterContaCorrenteResponse> ObterContaCorrente(ObterContaCorrenteRequest request)
        {
            var response = new ObterContaCorrenteResponse();

            using (var connection = new SqliteConnection(_dbConfig.Name))
            {
                await connection.OpenAsync();

                var querySql = @"
                SELECT idcontacorrente AS IdContaCorrente
                     , numero as Numero
                     , nome as Nome
                     , ativo as Ativo
                  FROM contacorrente 
                 WHERE idcontacorrente = @IdContaCorrente
                 ";

                var contaCorrente = await connection
                    .QueryFirstOrDefaultAsync<ContaCorrente>(querySql, new { IdContaCorrente = request.IdContaCorrente });

                if (contaCorrente != null)
                {
                    response.ContaCorrente = contaCorrente;
                    response.Sucesso = true;
                }
                else
                {
                    response.Sucesso = false;
                }
            }

            return response;
        }
    }
}