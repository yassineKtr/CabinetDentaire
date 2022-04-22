using DataAccess.Models;

namespace DataAccess.Writers.Consultations;

public interface IWriteConsultation
{
    Task AddConsultation(Consultation consultation);
    Task UpdateConsultation(Consultation consultation);
    Task DeleteConsultation(Guid consultation_id);
}