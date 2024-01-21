using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class MedicamentService
    {
        private ObjectDAO objectDAO;

        public MedicamentService()
        {
            objectDAO = new ObjectDAO();
        }

        public void saveMedicament(Medicament medicament)
        {
            int sequence = objectDAO.getViewSequence("next" + medicament.GetType().Name);
            string seq = objectDAO.getSequence(5, "MED", sequence);
            medicament.idMedicament = seq;
            objectDAO.insertObject(medicament);
        }

        public Medicament[] getAllMedicament()
        {
            Medicament medicament = new Medicament();
            Object[] objects = objectDAO.getObjects(medicament);
            Medicament[] medicaments = objects.Cast<Medicament>().ToArray();
            return medicaments;
        }

        public Medicament getByIdMedicament(string idMedicament)
        {
            Medicament medicament = new Medicament();
            Object[] objects = objectDAO.getObjectsByID(medicament, idMedicament);
            Medicament newMedicament = objects.Cast<Medicament>().First();
            return newMedicament;
        }
    }
}
