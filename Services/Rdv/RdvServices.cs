using DataAccess.Models;
using DataAccess.Readers.Consultations;
using DataAccess.Readers.Dentists;
using DataAccess.Readers.RendezVouss;
using DataAccess.Writers.RendezVouss;

namespace Services.Rdv
{
    public class RdvServices
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

        public async Task CreatRdv(string dentisteName,
                                    Guid clientId,
                                    string consultationType,
                                    DateTime date,
                                    bool paye)
        {
            var dentiste = await _dentisteReader.GetDentisteByName(dentisteName);
            var maxClients = dentiste.MaxClients;
            var currentClients = await _renderVousReader.GetRendezVousByDentisteId(dentiste.Dentiste_id);
            var currentClientsCount = currentClients.Count();
            if (currentClientsCount >= maxClients) => null; //TODO fix this
           
            var consultation = await _consultationReader.GetConsultationByType(consultationType);
            var rdv = new RendezVous
            {
                Client_id = clientId,
                Dentiste_id = dentiste.Dentiste_id,
                Consultation_id = consultation.Consultation_id,
                Date_rdv = date,
                Paye = paye
            }; 
            await _renderVousWriter.AddRendezVous(rdv);
        }
    }
}
