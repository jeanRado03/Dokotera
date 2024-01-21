using Dokotera.Services;
using System.Text;

namespace Dokotera.Models
{
    public class SymptomesPattient
    {
        public string[] symptomes { get; set; }
        public int[] valeur {  get; set; }
        public int age { get; set; }

        public string getSymptomesPatient()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < this.symptomes.Length; i++)
            {
                stringBuilder.Append("idsymptomes = '" + this.symptomes[i] + "'");
                stringBuilder.Append(" and ");
                stringBuilder.Append("valeurmin <= " + this.valeur[i]);
                stringBuilder.Append(" and ");
                stringBuilder.Append("valeurmax >= " + this.valeur[i]);
                stringBuilder.Append(" and ");
                stringBuilder.Append("agemin <= " + this.age);
                stringBuilder.Append(" and ");
                stringBuilder.Append("agemax >= " + this.age);
                stringBuilder.Append(" or ");
            }
            stringBuilder.Remove(stringBuilder.Length - 4, 4);
            stringBuilder.Append(" and ");
            stringBuilder.Append("agemin <= " + this.age);
            stringBuilder.Append(" and ");
            stringBuilder.Append("agemax >= " + this.age);
            return stringBuilder.ToString();
        }

        public Dictionary<string, List<InfoSymptomesMaladie>> getTest()
        {
            InfoSymptomesMaladie infoSymptomesMaladie = new InfoSymptomesMaladie();
            InfoSymptomesMaladieService infoSymptomesMaladieService = new InfoSymptomesMaladieService();
            return infoSymptomesMaladie.donnees(infoSymptomesMaladieService, this.getSymptomesPatient());
        }

        public Dictionary<string, List<InfoSymptomesMaladie>> getListesMaladies()
        {
            InfoSymptomesMaladie infoSymptomesMaladie = new InfoSymptomesMaladie();
            InfoSymptomesMaladieService infoSymptomesMaladieService = new InfoSymptomesMaladieService();
            Dictionary<string, List<InfoSymptomesMaladie>> keyValues = infoSymptomesMaladie.donnees(infoSymptomesMaladieService, this.getSymptomesPatient());
            List<string> list = new List<string>(keyValues.Keys);
            Dictionary<string, List<InfoSymptomesMaladie>> result = new Dictionary<string, List<InfoSymptomesMaladie>>();
            foreach (string key in list)
            {
                List<InfoSymptomesMaladie> trueSymptomes = infoSymptomesMaladieService.getInfoByMaladieAndAgeAsc(keyValues[key].First().idMaladie, this.age).ToList();
                bool sontEgales = comparaison(keyValues[key], trueSymptomes);
                Console.WriteLine("Comparaison : "+sontEgales);
                if (!sontEgales)
                {
                    continue;
                }
                result.Add(key, keyValues[key]);
            }
            return result;
        }

        bool comparaison(List<InfoSymptomesMaladie> info1, List<InfoSymptomesMaladie> info2)
        {
            if(info1.Count != info2.Count)
            {
                Console.WriteLine("Tsy mitovy ny taille");
                return false;
            }
            for (int i = 0; i < info1.Count; i++)
            {
                if (!info1[i].Equals(info2[i]))
                {
                    Console.WriteLine("Tsy mitovy ilay objet");
                    return false;
                }
            }
            return true;
        }
    }
}
