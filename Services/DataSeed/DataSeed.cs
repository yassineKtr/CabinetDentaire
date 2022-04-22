using DataAccess.Models;
using DataAccess.Readers.Dentists;
using DataAccess.Writers.Consultations;
using DataAccess.Writers.Dentistes;

namespace Services.DataSeed
{
    public class DataSeed
    {
        public static async Task SeedDentiste(IWriteDentiste dentisteWriter)
        {
            var dentiste1 = new Dentiste
            {
                Dentiste_id = Guid.NewGuid(),
                Nom = "Dentiste 1",
                Debut_travail = 8,
                Fin_travail = 11,
                Max_clients = 10
            };
            var dentiste2 = new Dentiste
            {
                Dentiste_id = Guid.NewGuid(),
                Nom = "Dentiste 2",
                Debut_travail = 1130,
                Fin_travail = 14,
                Max_clients = 3
            };
            var dentiste3 = new Dentiste
            {
                Dentiste_id = Guid.NewGuid(),
                Nom = "Dentiste 3",
                Debut_travail = 1430,
                Fin_travail = 17,
                Max_clients = 7
            };

            await dentisteWriter.AddDentiste(dentiste1);
            await dentisteWriter.AddDentiste(dentiste2);
            await dentisteWriter.AddDentiste(dentiste3);


        }

        public static async Task SeedConsultation(IWriteConsultation consultationWriter)
        {
            var consultation1 = new Consultation
            {
                Consultation_id = Guid.NewGuid(),
                Consultation_type = "Detartrage ",
                Prix = 200
            };
            var consultation2 = new Consultation
            {
                Consultation_id = Guid.NewGuid(),
                Consultation_type = "Traitement d’une carie ",
                Prix = 350
            };
            var consultation3 = new Consultation
            {
                Consultation_id = Guid.NewGuid(),
                Consultation_type = "Devitalisation d’une incisive ou d’une canine ",
                Prix = 450
            };
            var consultation4 = new Consultation
            {
                Consultation_id = Guid.NewGuid(),
                Consultation_type = "Extraction d’une dent",
                Prix = 800
            };
            await consultationWriter.AddConsultation(consultation1);
            await consultationWriter.AddConsultation(consultation2);
            await consultationWriter.AddConsultation(consultation3);
            await consultationWriter.AddConsultation(consultation4);
        }
    }
}
