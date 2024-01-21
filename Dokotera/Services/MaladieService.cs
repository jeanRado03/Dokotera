using Dokotera.DataAccess;
using Dokotera.Models;

namespace Dokotera.Services
{
    public class MaladieService
    {
        private ObjectDAO objectDAO;

        public MaladieService()
        {
            objectDAO = new ObjectDAO();
        }

        public void saveMaladie(Maladie maladie)
        {
            int sequence = objectDAO.getViewSequence("next" + maladie.GetType().Name);
            string seq = objectDAO.getSequence(5, "MD", sequence);
            maladie.idMaladie = seq;
            objectDAO.insertObject(maladie);
        }

        public Maladie[] getAllMaladie()
        {
            Maladie maladie = new Maladie();
            Object[] objects = objectDAO.getObjects(maladie);
            Maladie[] maladies = objects.Cast<Maladie>().ToArray();
            return maladies;
        }

        public Maladie getByIdMaladie(string idMaladie)
        {
            Maladie maladie = new Maladie();
            Object[] objects = objectDAO.getObjectsByID(maladie, idMaladie);
            Maladie newMaladie = objects.Cast<Maladie>().First();
            return newMaladie;
        }
    }
}
