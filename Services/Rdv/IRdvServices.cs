using DataAccess.Models;

namespace Services.Rdv;

public interface IRdvServices
{
    Task CreateRdv(string dentisteName,
        Guid clientId,
        string consultationType,
        DateTime date);

    Task CancelRdv(Guid rdv_id, string reason);
    Task PayForRdv(Guid rdv_id);
}