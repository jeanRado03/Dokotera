using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class TraitementSymptomesService
    {
        private ObjectDAO objectDAO;

        public TraitementSymptomesService()
        {
            objectDAO = new ObjectDAO();
        }

        public TraitementSymptomes[] getAllTraitementSymptomes()
        {
            TraitementSymptomes traitementSymptomes = new TraitementSymptomes();
            Object[] objects = objectDAO.getView(traitementSymptomes);
            TraitementSymptomes[] traitementSymptomess = objects.Cast<TraitementSymptomes>().ToArray();
            return traitementSymptomess;
        }

        public TraitementSymptomes[] getAllTraitementSymptomesBetween(int age)
        {
            TraitementSymptomes traitementSymptomes = new TraitementSymptomes();
            Object[] objects = objectDAO.getViewBetweenOrderBy(traitementSymptomes, age, "idsymptomes", "desc");
            TraitementSymptomes[] traitementSymptomess = objects.Cast<TraitementSymptomes>().ToArray();
            return traitementSymptomess;
        }

        public ValeurSymptomes getValeurSymptomes(string idmedic, string idsympt)
        {
            ValeurSymptomes valeurSymptomes = new ValeurSymptomes();
            Object[] objects = objectDAO.getValeurSymptome(valeurSymptomes,idmedic, idsympt);
            ValeurSymptomes newVal = new ValeurSymptomes() { valeurTraitemet = 0 };
            if (objects.Length > 0)
            {
                newVal = objects.Cast<ValeurSymptomes>().First();
            }
            return newVal;
        }
    }
}
