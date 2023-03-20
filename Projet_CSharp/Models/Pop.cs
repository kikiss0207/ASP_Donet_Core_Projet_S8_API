using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_CSharp.Models
{
    public class Pop
    {
        public int Id { get; set; }

        public int Nbre_pop { get; set; }

        public int Annee { get; set; }


        [ForeignKey("Countrie")] public int Id_country { get; set; }
    }
}
