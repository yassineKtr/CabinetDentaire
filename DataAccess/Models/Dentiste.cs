namespace DataAccess.Models
{
    public class Dentiste
    {
        public Guid Dentiste_id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Debut_travail { get; set; }
        public int Fin_travail { get; set; }
        public int Max_clients { get; set; }

    }
}
