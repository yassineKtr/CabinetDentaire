using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Readers.RendezVouss
{
    public class RendezVousReader : IReadRendezVous
    {
        private readonly PostgresqlConfig _config;
        private readonly IPostgresqlConnection _connection;

        public RendezVousReader(IConfiguration config)
        {
            _config = new PostgresqlConfig(config);
            _connection = new PostgresqlConnection(_config);
        }

        public async Task<IEnumerable<RendezVous>> GetAllRendezVous()
        {
            var sql = "SELECT * FROM rendezvous";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<RendezVous>(sql,new{});
        }
        public async Task<RendezVous?> GetRendezVousById(Guid id)
        {
            var sql = "SELECT * FROM rendezvous WHERE rdv_id = @id";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<RendezVous>(sql, new { id });
        }

        public async Task<IEnumerable<RendezVous>> GetRendezVousByDate(DateTime date)
        {
            var sql = "SELECT * FROM rendezvous WHERE date_rdv = @date";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<RendezVous>(sql, new { date });
        }

        public async Task<IEnumerable<RendezVous>> GetRendezVousByClientId(Guid id)
        {
            var sql = "SELECT * FROM rendezvous WHERE client_id = @id";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<RendezVous>(sql, new { id });
        }

        public async Task<IEnumerable<RendezVous>> GetRendezVousByDentisteId(Guid id)
        {
            var sql = "SELECT * FROM rendezvous WHERE dentiste_id = @id";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<RendezVous>(sql, new { id });
        }
        public async Task<IEnumerable<RendezVous>> GetRendezVousByConsultationId(Guid id)
        {
            var sql = "SELECT * FROM rendezvous WHERE consultation_id = @id";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<RendezVous>(sql, new { id });
        }
    }
}
