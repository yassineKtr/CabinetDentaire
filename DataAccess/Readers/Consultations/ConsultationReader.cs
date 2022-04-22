using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Readers.Consultations
{
    public class ConsultationReader : IReadConsultation
    {
        private readonly PostgresqlConfig _config;
        private readonly IPostgresqlConnection _connection;

        public ConsultationReader(IConfiguration config)
        {
            _config = new PostgresqlConfig(config);
            _connection = new PostgresqlConnection(_config);
        }
        public async Task<IEnumerable<Consultation>> GetConsultations()
        {
            var sql = "SELECT * FROM consultation";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<Consultation>(sql,new{});
            
        }
        public async Task<Consultation?> GetConsultationById(Guid id)
        {
            var sql = "SELECT * FROM consultation WHERE consultation_id = @id";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<Consultation>(sql, new { id });
        }
        public async Task<Consultation?> GetConsultationByType(string type)
        {
            var sql = "SELECT * FROM consultation WHERE consultation_type = @type";
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<Consultation>(sql, new { type });
        }

    }
}
