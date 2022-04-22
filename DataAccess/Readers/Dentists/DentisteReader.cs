using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Readers.Dentists
{
    public class DentisteReader : IReadDentiste
    {
        private readonly PostgresqlConfig _config;
        private readonly IPostgresqlConnection _connection;

        public DentisteReader(IConfiguration config)
        {
            _config = new PostgresqlConfig(config);
            _connection = new PostgresqlConnection(_config);
        }

        public async Task<IEnumerable<Dentiste>> GetDentistes()
        {
            var sql = "SELECT * FROM dentiste";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<Dentiste>(sql, new { });
        }
        public async Task<Dentiste?> GetDentisteById(Guid id)
        {
            var sql = "SELECT * FROM dentiste WHERE dentiste_id = @id";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<Dentiste?>(sql, new { id });
        }

        public async Task<Dentiste?> GetDentisteByName(string name)
        {
            var sql = "SELECT * FROM dentiste WHERE nom=@name";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            var dentiste = await connection.QueryFirstOrDefaultAsync<Dentiste>(sql, new { name });
            return dentiste;
        }
    }
}
