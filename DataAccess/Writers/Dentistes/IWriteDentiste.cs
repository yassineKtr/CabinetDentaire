using DataAccess.Models;

namespace DataAccess.Writers.Dentistes;

public interface IWriteDentiste
{
    Task AddDentiste(Dentiste dentiste);
    Task UpdateDentiste(Dentiste dentiste);
    Task DeleteDentiste(Guid id);
}