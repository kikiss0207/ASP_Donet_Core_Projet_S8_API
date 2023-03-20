using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_CSharp.Models
{
    public class Countrie
    {
        public int Id { get; set; }
        public string Country_name { get; set; }

        public ICollection<Pop> List_pop { get; set; }
        [ForeignKey("Continent")] public int Id_Continent { get; set; }
    }
}
