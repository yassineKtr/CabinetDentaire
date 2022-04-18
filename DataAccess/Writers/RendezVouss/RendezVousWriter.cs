using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Writers.RendezVouss
{
    public class RendezVousWriter : IWriteRendezVous
    {
        private readonly PostgresqlConfig _config;
        private readonly IPostgresqlConnection _connection;

        public RendezVousWriter(IConfiguration config)
        {
            _config = new PostgresqlConfig(config);
            _connection = new PostgresqlConnection(_config);
        }

        public async Task AddRendezVous(RendezVous rendezVous)
        {
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();            
            var sql = "INSERT INTO rendezvous (rdv_id, date_rdv, annule, reason, dentiste_id,consultation_id,client_id,paye) " +
                      "VALUES (@id, @date, @annule, @reason, @dentisteId, @consultationId, @clientId, @paye)";
            var parameters = new
            {
                id = rendezVous.Rdv_id,
                date = rendezVous.Date_rdv,
                annule = rendezVous.Annule,
                reason = rendezVous.Reason,
                dentisteId = rendezVous.Dentiste_id,
                consultationId = rendezVous.Consultation_id,
                clientId = rendezVous.Client_id,
                paye = rendezVous.Paye
            };
            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task UpdateRendezVous(RendezVous rdv)
        {
            var query = "UPDATE rendezvous " +
                        "SET date_rdv = @date, " +
                        " annule = @annule, " +
                        "reason = @reason, " +
                        "dentiste_id = @dentisteId, " +
                        "consultation_id = @consultationId, " +
                        "client_id = @clientId, " +
                        "paye = @paye " +
                        "WHERE rdv_id = @id";
            var parameters = new
            {
                id = rdv.Rdv_id,
                date = rdv.Date_rdv,
                annule = rdv.Annule,
                reason = rdv.Reason,
                dentisteId = rdv.Dentiste_id,
                consultationId = rdv.Consultation_id,
                clientId = rdv.Client_id,
                paye = rdv.Paye
            };
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteRendezVous(Guid id)
        {
            var query = "DELETE FROM rendezvous WHERE rdv_id = @id";
            var parameters = new {id = id};
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }


    }
}
