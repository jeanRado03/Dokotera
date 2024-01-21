using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class SymptomesMaladieService
    {
        private ObjectDAO objectDAO;

        public SymptomesMaladieService() 
        {
            objectDAO = new ObjectDAO();
        }
        public void saveObjectsSymptomesMaladie(SymptomesMaladie[] symptomesMaladies)
        {
            for (int i = 0; i < symptomesMaladies.Length; i++)
            {
                int sequence = objectDAO.getViewSequence("next" + symptomesMaladies[i].GetType().Name);
                string seq = objectDAO.getSequence(5, "SPMD", sequence);
                symptomesMaladies[i].idSymptomesMaladie = seq;
            }
            objectDAO.insertObjects(symptomesMaladies);
        }


        public void saveSymptomesMaladie(SymptomesMaladie symptomesMaladie)
        {
            int sequence = objectDAO.getViewSequence("next" + symptomesMaladie.GetType().Name);
            string seq = objectDAO.getSequence(5, "SPMD", sequence);
            symptomesMaladie.idSymptomesMaladie = seq;
            objectDAO.insertObject(symptomesMaladie);
        }

        public SymptomesMaladie[] getAllSymptomesMaladie()
        {
            SymptomesMaladie symptomesMaladie = new SymptomesMaladie();
            Object[] objects = objectDAO.getObjects(symptomesMaladie);
            SymptomesMaladie[] symptomesMaladies = objects.Cast<SymptomesMaladie>().ToArray();
            return symptomesMaladies;
        }

        public SymptomesMaladie getByIdSymptomesMaladie(string idSymptomesMaladie)
        {
            SymptomesMaladie symptomesMaladie = new SymptomesMaladie();
            Object[] objects = objectDAO.getObjectsByID(symptomesMaladie, idSymptomesMaladie);
            SymptomesMaladie symptomesMaladies = objects.Cast<SymptomesMaladie>().First();
            return symptomesMaladies;
        }
    }
}
