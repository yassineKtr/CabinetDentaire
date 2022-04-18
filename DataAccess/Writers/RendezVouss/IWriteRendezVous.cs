using DataAccess.Models;

namespace DataAccess.Writers.RendezVouss;

public interface IWriteRendezVous
{
    Task AddRendezVous(RendezVous rendezVous);
    Task UpdateRendezVous(RendezVous rdv);
    Task DeleteRendezVous(Guid id);
}