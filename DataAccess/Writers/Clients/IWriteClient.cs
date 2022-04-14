using DataAccess.Models;

namespace DataAccess.Writers.Clients;

public interface IWriteClient
{
    Task AddClient(Client client);
    Task UpdateClient(Client client);
    Task DeleteClient(Guid client_id);
}