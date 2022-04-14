using DataAccess.Models;

namespace DataAccess.Readers.Clients;

public interface IReadClient
{
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client?> GetClientById(Guid id);
}