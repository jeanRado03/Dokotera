using Dokotera.Services;

namespace Dokotera.Models
{
    public class InfoSymptomesMaladie
    {
        public string idMaladie { get; set; }
        public string nomMaladie { get; set; }
        public string idSymptomes { get; set; }
        public string nomSymptomes { get; set; }
        public int valeurMin { get; set; }
        public int valeurMax { get; set; }
        public int ageMin { get; set; }
        public int ageMax { get; set; }

        public bool Equals(InfoSymptomesMaladie other)
        {
            return (idMaladie == other.idMaladie && nomMaladie == other.nomMaladie && idSymptomes ==  other.idSymptomes && nomSymptomes == other.nomSymptomes && valeurMin == other.valeurMin && valeurMax == other.valeurMax && ageMin == other.ageMin && ageMax == other.ageMax);
        }

        public Dictionary<string, List<InfoSymptomesMaladie>> donnees (InfoSymptomesMaladieService infoSymptomesMaladieService, string condtitonSymptomes)
        {
            Dictionary<string, List<InfoSymptomesMaladie>> dict = new Dictionary<string, List<InfoSymptomesMaladie>> ();
            List<InfoSymptomesMaladie> infoSymptomesMaladies = new List<InfoSymptomesMaladie> ();
            InfoSymptomesMaladie[] infoSymptomes = infoSymptomesMaladieService.getInfoBySymptomesAsc(condtitonSymptomes);
            int i = 0;
            foreach(InfoSymptomesMaladie info in  infoSymptomes) 
            {
                Console.WriteLine(info.nomMaladie);
                if (i == infoSymptomes.Length - 1 && infoSymptomesMaladies.Any(m => m.idMaladie == info.idMaladie))
                {
                    infoSymptomesMaladies.Add(info);
                    dict.Add(infoSymptomesMaladies.Last().nomMaladie, infoSymptomesMaladies);
                    break;
                }
                else if(i == infoSymptomes.Length - 1 && !infoSymptomesMaladies.Any(m => m.idMaladie == info.idMaladie))
                {
                    dict.Add(infoSymptomesMaladies.Last().nomMaladie, infoSymptomesMaladies);
                    infoSymptomesMaladies = new List<InfoSymptomesMaladie>();
                    infoSymptomesMaladies.Add(info);
                    dict.Add(infoSymptomesMaladies.Last().nomMaladie, infoSymptomesMaladies);
                    break;
                }
                if (infoSymptomesMaladies.Count == 0 || infoSymptomesMaladies.Any(m => m.idMaladie == info.idMaladie))
                {
                    infoSymptomesMaladies.Add(info);
                    i++;
                    continue;
                }
                dict.Add(infoSymptomesMaladies.Last().nomMaladie, infoSymptomesMaladies);
                Console.WriteLine("eto @ ilay miova : " + info.idMaladie);
                infoSymptomesMaladies = new List<InfoSymptomesMaladie>();
                infoSymptomesMaladies.Add(info);
                i++;
            }
            return dict;
        }
    }
}
