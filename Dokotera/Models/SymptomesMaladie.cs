namespace Dokotera.Models
{
    public class SymptomesMaladie
    {
        public string idSymptomesMaladie { get; set; }
        public string idMaladie { get; set; }
        public string idSymptomes { get; set; }
        public int valeurMin { get; set; }
        public int valeurMax { get; set; }
        public int ageMin { get; set; }
        public int ageMax { get; set; }
    }
}
