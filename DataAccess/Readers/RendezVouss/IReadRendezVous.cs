using DataAccess.Models;

namespace DataAccess.Readers.RendezVouss;

public interface IReadRendezVous
{
    Task<IEnumerable<RendezVous>> GetAllRendezVous();
    Task<RendezVous?> GetRendezVousById(Guid id);
    Task<IEnumerable<RendezVous>> GetRendezVousByDate(DateTime date);
    Task<IEnumerable<RendezVous>> GetRendezVousByClientId(Guid id);
    Task<IEnumerable<RendezVous>> GetRendezVousByDentisteId(Guid id);
    Task<IEnumerable<RendezVous>> GetRendezVousByConsultationId(Guid id);
}