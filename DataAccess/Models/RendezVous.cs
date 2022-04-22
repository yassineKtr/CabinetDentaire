namespace DataAccess.Models
{
    public class RendezVous
    {
        public Guid Rdv_id { get; set; }
        public Guid Dentiste_id { get; set; }
        public Guid  Client_id { get; set; }
        public Guid  Consultation_id { get; set; }
        public DateTime Date_rdv { get; set; }
        public bool Annule { get; set; } = false;
        public string Reason { get; set; } = "";
        public bool Paye { get; set; } = false;
    }
}
