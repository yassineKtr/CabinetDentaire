using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Writers.Dentistes
{
    public class DentisteWriter : IWriteDentiste
    {
        private readonly PostgresqlConfig _config;
        private readonly IPostgresqlConnection _connection;

        public DentisteWriter(IConfiguration config)
        {
            _config = new PostgresqlConfig(config);
            _connection = new PostgresqlConnection(_config);
        }

        public async Task AddDentiste(Dentiste dentiste)
        {
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            var query = "INSERT INTO dentiste (dentiste_id, nom, prenom, debut_travail, fin_travail, max_clients)" +
                        " VALUES (@id, @nom, @prenom, @debut_travail, @fin_travail, @max_clients)";
            var parameters = new
            {
                id = dentiste.Dentiste_id,
                nom = dentiste.Nom,
                prenom = dentiste.Prenom,
                debut_travail = dentiste.Debut_travail,
                fin_travail = dentiste.Fin_travail,
                max_clients = dentiste.Max_clients
            };
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateDentiste(Dentiste dentiste)
        {
            var query = "UPDATE dentiste " +
                        "SET nom = @nom, prenom = @prenom, debut_travail = @debut_travail, fin_travail = @fin_travail, max_clients = @max_clients" +
                        " WHERE dentiste_id = @id";
            var parameters = new
            {
                id = dentiste.Dentiste_id,
                nom = dentiste.Nom,
                prenom = dentiste.Prenom,
                debut_travail = dentiste.Debut_travail,
                fin_travail = dentiste.Fin_travail,
                max_clients = dentiste.Max_clients
            };
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteDentiste(Guid id)
        {
            var query = "DELETE FROM dentiste WHERE dentiste_id = @id";
            var parameters = new { id = id };
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }

    }
}
