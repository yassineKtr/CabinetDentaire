using DataAccess.Models;

namespace DataAccess.Readers.Consultations;

public interface IReadConsultation
{
    Task<IEnumerable<Consultation>> GetConsultations();
    Task<Consultation?> GetConsultationById(Guid id);
    Task<Consultation?> GetConsultationByType(string type);
}