using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class PrixService
    {
        private ObjectDAO objectDAO;

        public PrixService() 
        {
            objectDAO = new ObjectDAO();
        }

        public void savePrix(Prix prix)
        {
            int sequence = objectDAO.getViewSequence("next" + prix.GetType().Name);
            string seq = objectDAO.getSequence(5, "PRX", sequence);
            prix.idPrix = seq;
            objectDAO.insertObject(prix);
        }

        public Prix[] getAllPrix()
        {
            Prix prix = new Prix();
            Object[] objects = objectDAO.getObjects(prix);
            Prix[] prixs = objects.Cast<Prix>().ToArray();
            return prixs;
        }

        public Prix getByIdPrix(string idPrix)
        {
            Prix prix = new Prix();
            Object[] objects = objectDAO.getObjectsByID(prix, idPrix);
            Prix newPrix = objects.Cast<Prix>().First();
            return newPrix;
        }
    }
}
