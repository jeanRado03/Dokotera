using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class MedicamentSymptomesService
    {
        private ObjectDAO objectDAO;

        public MedicamentSymptomesService() 
        {
            objectDAO = new ObjectDAO();
        }

        public void saveMedicamentSymptomes(MedicamentSymptomes medicamentSymptomes)
        {
            int sequence = objectDAO.getViewSequence("next" + medicamentSymptomes.GetType().Name);
            string seq = objectDAO.getSequence(5, "MEDS", sequence);
            medicamentSymptomes.idMedicamentSymptomes = seq;
            objectDAO.insertObject(medicamentSymptomes);
        }

        public MedicamentSymptomes[] getAllMedicamentSymptomes()
        {
            MedicamentSymptomes medicamentSymptomes = new MedicamentSymptomes();
            Object[] objects = objectDAO.getObjects(medicamentSymptomes);
            MedicamentSymptomes[] medicamentSymptomess = objects.Cast<MedicamentSymptomes>().ToArray();
            return medicamentSymptomess;
        }

        public MedicamentSymptomes getByIdMedicamentSymptomes(string idMedicamentSymptomes)
        {
            MedicamentSymptomes medicamentSymptomes = new MedicamentSymptomes();
            Object[] objects = objectDAO.getObjectsByID(medicamentSymptomes, idMedicamentSymptomes);
            MedicamentSymptomes newMedicamentSymptomes = objects.Cast<MedicamentSymptomes>().First();
            return newMedicamentSymptomes;
        }
    }
}
