using DataAccess.Models;

namespace Services.Rdv;

public interface IRdvServices
{
    Task<Tuple<string, RendezVous>> CreateRdv(string dentisteName,
        Guid clientId,
        string consultationType,
        DateTime date);

    Task CancelRdv(Guid rdv_id, string reason);
    Task<string> PayForRdv(Guid rdv_id);
}