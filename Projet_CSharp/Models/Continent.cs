using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Projet_CSharp.Models
{
    public class Continent
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }
        public ICollection<Countrie> List_country { get; set; }
    }
}
