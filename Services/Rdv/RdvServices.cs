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

        public async Task<Tuple<string,RendezVous>> CreateRdv(string dentisteName,
                                        Guid clientId,
                                        string consultationType,
                                        DateTime date)
        {
            var dentiste = await _dentisteReader.GetDentisteByName(dentisteName);
            if (dentiste == null) return Tuple.Create<string,RendezVous>("Dentiste not found", null);
            var maxClients = dentiste.Max_clients;
            var currentClients = await _renderVousReader.GetRendezVousByDentisteId(dentiste.Dentiste_id);
            var currentClientsCount = currentClients.Count();
            if (currentClientsCount >= maxClients) return Tuple.Create<string, RendezVous>("Dentiste full",null) ; 
           
            var consultation = await _consultationReader.GetConsultationByType(consultationType);
            if (consultation == null) return Tuple.Create<string, RendezVous>("consultation not found",null);
            var rdv = new RendezVous
            {
                Client_id = clientId,
                Dentiste_id = dentiste.Dentiste_id,
                Consultation_id = consultation.Consultation_id,
                Date_rdv = date,
            }; 
            await _renderVousWriter.AddRendezVous(rdv);
            return Tuple.Create<string, RendezVous>("Rdv created", rdv);
        }

        public async Task CancelRdv(Guid rdv_id, string reason)
        {
            //TODO: fix tests for this method 
            var rdv = await _renderVousReader.GetRendezVousById(rdv_id);
            if (rdv == null) throw new Exception("Rendez_vous not found");//return "Rendez_vous not found";
            rdv.Annule = true;
            rdv.Reason = reason;
            await _renderVousWriter.UpdateRendezVous(rdv);
        }

        public async Task<string> PayForRdv(Guid rdv_id)
        {
            var rdv = await _renderVousReader.GetRendezVousById(rdv_id);
            if (rdv == null) return "Rendez_vous not found";
            rdv.Paye = true;
            await _renderVousWriter.UpdateRendezVous(rdv);
            return "Rendez_vous payed";
        }

    }
}
