using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Writers.Consultations
{
    public class ConsultationWriter : IWriteConsultation
    {
        private readonly PostgresqlConfig _config;
        private readonly IPostgresqlConnection _connection;

        public ConsultationWriter(IConfiguration config)
        {
            _config = new PostgresqlConfig(config);
            _connection = new PostgresqlConnection(_config);
        }

        public async Task AddConsultation(Consultation consultation)
        {
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            var query = "INSERT INTO consultation (consultation_id,consultation_type,prix) " +
                        "VALUES (@consultation_id,@consultation_type,@prix)";
            var parameters = new
            {
                consultation_id = consultation.Consultation_id,
                consultation_type = consultation.Consultation_type,
                prix = consultation.Prix
            };
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateConsultation(Consultation consultation)
        {
            var query = "UPDATE consultation SET consultation_type = @consultation_type, prix = @prix " +
                        "WHERE consultation_id = @consultation_id";
            var parameters = new
            {
                consultation_id = consultation.Consultation_id,
                consultation_type = consultation.Consultation_type,
                prix = consultation.Prix
            };
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }
        public async Task DeleteConsultation(Guid consultation_id)
        {
            var query = "DELETE FROM consultation WHERE consultation_id = @consultation_id";
            var parameters = new { consultation_id };
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
