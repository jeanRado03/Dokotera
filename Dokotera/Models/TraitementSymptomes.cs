namespace Dokotera.Models
{
    public class TraitementSymptomes
    {
        public string idMedicament { get; set; }
        public string nommedicament { get; set; }
        public string idSymptomes { get; set; }
        public string nomSymptomes { get; set; }
        public int valeurTraitemet { get; set; }
        public int ageMin { get; set; }
        public int ageMax { get; set; }
        public double prixUnitaire { get; set; }
    }
}
