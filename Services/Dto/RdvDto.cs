namespace Services.Helpers
{
    public class RdvDto
    {
        public string DentisteName{ get; set; }
        public Guid ClientId { get; set; }
        public string ConsultationType { get; set; }
        public DateTime Date { get; set; }
    }
}
