using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class SymptomesService
    {
        private ObjectDAO objectDAO;

        public SymptomesService()
        {
            objectDAO = new ObjectDAO();
        }

        public void saveSymptomes(Symptomes symptomes)
        {
            int sequence = objectDAO.getViewSequence("next" + symptomes.GetType().Name);
            string seq = objectDAO.getSequence(5, "SPT", sequence);
            symptomes.idSymptomes = seq;
            objectDAO.insertObject(symptomes);
        }

        public Symptomes[] getAllSymptomes()
        {
            Symptomes symptomes = new Symptomes();
            Object[] objects = objectDAO.getObjects(symptomes);
            Symptomes[] symptomess = objects.Cast<Symptomes>().ToArray();
            return symptomess;
        }

        public Symptomes getByIdSymptomes(string idSymptomes)
        {
            Symptomes symptomes = new Symptomes();
            Object[] objects = objectDAO.getObjectsByID(symptomes, idSymptomes);
            Symptomes newSymptomes = objects.Cast<Symptomes>().First();
            return newSymptomes;
        }
    }
}
