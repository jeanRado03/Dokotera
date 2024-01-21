using System.Runtime.CompilerServices;

namespace Dokotera.Models
{
    public class Medicament
    {
        public string idMedicament {  get; set; }
        public string nomMedicament { get; set; }
        public int ageMin { get; set; }
        public int ageMax { get; set; }
    }
}
