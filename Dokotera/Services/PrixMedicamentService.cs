using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class PrixMedicamentService
    {
        private ObjectDAO objectDAO;

        public PrixMedicamentService()
        {
            objectDAO = new ObjectDAO();
        }

        public void savePrixMedicament(PrixMedicament prixMedicament)
        {
            int sequence = objectDAO.getViewSequence("next" + prixMedicament.GetType().Name);
            string seq = objectDAO.getSequence(5, "PRXM", sequence);
            prixMedicament.idPrixMedicament = seq;
            objectDAO.insertObject(prixMedicament);
        }

        public PrixMedicament[] getAllPrixMedicament()
        {
            PrixMedicament prixMedicament = new PrixMedicament();
            Object[] objects = objectDAO.getObjects(prixMedicament);
            PrixMedicament[] prixMedicaments = objects.Cast<PrixMedicament>().ToArray();
            return prixMedicaments;
        }

        public PrixMedicament getByIdPrixMedicament(string idPrixMedicament)
        {
            PrixMedicament prixMedicament = new PrixMedicament();
            Object[] objects = objectDAO.getObjectsByID(prixMedicament, idPrixMedicament);
            PrixMedicament newPrixMedicament = objects.Cast<PrixMedicament>().First();
            return newPrixMedicament;
        }
    }
}
