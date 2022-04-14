using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Writers.Clients
{
    public class ClientWriter : IWriteClient
    {
        private readonly PostgresqlConfig _config;
        private readonly IPostgresqlConnection _connection;

        public ClientWriter(IConfiguration config)
        {
            _config = new PostgresqlConfig(config);
            _connection = new PostgresqlConnection(_config);
        }

        public async Task AddClient(Client client)
        {
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            var query = "INSERT INTO client (client_id, nom, prenom, email, telephone) " +
                        "VALUES (@id, @nom, @prenom, @email, @telephone)";
            var parameters = new 
            {
                id=client.Client_id,
                nom= client.Nom,
                prenom= client.Prenom,
                email= client.Email,
                telephone= client.Telephone
            };
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateClient(Client client)
        {
            var query = "UPDATE client SET nom = @nom, prenom = @prenom, email = @email, telephone = @telephone " +
                        "WHERE client_id = @id";
            var parameters = new
            {
                id = client.Client_id,
                nom = client.Nom,
                prenom = client.Prenom,
                email = client.Email,
                telephone = client.Telephone
            };
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }
        public async Task DeleteClient(Guid client_id)
        {
            var query = "DELETE FROM client WHERE client_id = @id";
            var parameters = new
            {
                id = client_id
            };
            await using var connection = _connection.GetSqlConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
