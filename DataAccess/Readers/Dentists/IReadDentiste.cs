using DataAccess.Models;

namespace DataAccess.Readers.Dentists;

public interface IReadDentiste
{
    Task<IEnumerable<Dentiste>> GetDentistes();
    Task<Dentiste?> GetDentisteById(Guid id);
    Task<Dentiste?> GetDentisteByName(string name);
}