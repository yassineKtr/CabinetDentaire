namespace DataAccess.Models
{
    public class Client
    {
        public Guid Client_id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
    }
}
