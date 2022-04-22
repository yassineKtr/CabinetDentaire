using DataAccess.Models;
using DataAccess.Readers.Consultations;
using DataAccess.Readers.Dentists;
using DataAccess.Readers.RendezVouss;
using DataAccess.Writers.RendezVouss;

namespace Services.Rdv
{
    public class RdvServices : IRdvServices
    {
        private readonly IWriteRendezVous _renderVousWriter;
        private readonly IReadRendezVous _renderVousReader;
        private readonly IReadConsultation _consultationReader;
        private readonly IReadDentiste _dentisteReader;


        public RdvServices(IWriteRendezVous renderVousWriter, 
                            IReadRendezVous renderVousReader,
                            IReadConsultation consultationReader,
                            IReadDentiste dentisteReader)
        {
            _renderVousWriter = renderVousWriter;
            _renderVousReader = renderVousReader;
            _consultationReader = consultationReader;
            _dentisteReader = dentisteReader;
        }

        public async Task CreateRdv(string dentisteName,
                                        Guid clientId,
                                        string consultationType,
                                        DateTime date)
        {
            var dentiste = await _dentisteReader.GetDentisteByName(dentisteName);
            if (dentiste == null) throw new Exception("Dentiste not found");
            var maxClients = dentiste.Max_clients;
            var currentClients = await _renderVousReader.GetRendezVousByDentisteId(dentiste.Dentiste_id);
            var currentClientsCount = currentClients.Count();
            if (currentClientsCount >= maxClients) throw new Exception("Dentiste is full");

            var consultation = await _consultationReader.GetConsultationByType(consultationType);
            if (consultation == null) throw new Exception("Consultation not found");
            var rdv = new RendezVous
            {
                Client_id = clientId,
                Dentiste_id = dentiste.Dentiste_id,
                Consultation_id = consultation.Consultation_id,
                Date_rdv = date,
            }; 
            await _renderVousWriter.AddRendezVous(rdv);
        }

        public async Task CancelRdv(Guid rdv_id, string reason)
        {
            var rdv = await _renderVousReader.GetRendezVousById(rdv_id);
            if (rdv == null) throw new Exception("Rendez_vous not found");
            rdv.Annule = true;
            rdv.Reason = reason;
            await _renderVousWriter.UpdateRendezVous(rdv);
        }

        public async Task PayForRdv(Guid rdv_id)
        {
            var rdv = await _renderVousReader.GetRendezVousById(rdv_id);
            if (rdv == null) throw new Exception("Rendez_vous not found");
            rdv.Paye = true;
            await _renderVousWriter.UpdateRendezVous(rdv);
        }

    }
}
