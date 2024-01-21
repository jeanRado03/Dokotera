using Dokotera.DataAccess;
using Dokotera.Models;
using Microsoft.VisualBasic;

namespace Dokotera.Services
{
    public class InfoSymptomesMaladieService
    {
        private ObjectDAO objectDAO;

        public InfoSymptomesMaladieService()
        {
            objectDAO = new ObjectDAO();
        }

        public InfoSymptomesMaladie[] getAllInfoSymptomesMaladie()
        {
            InfoSymptomesMaladie infoSymptomesMaladie = new InfoSymptomesMaladie();
            Object[] objects = objectDAO.getView(infoSymptomesMaladie);
            InfoSymptomesMaladie[] infoSymptomesMaladies = objects.Cast<InfoSymptomesMaladie>().ToArray();
            return infoSymptomesMaladies;
        }

        public InfoSymptomesMaladie[] getInfoAsc()
        {
            InfoSymptomesMaladie infoSymptomesMaladie = new InfoSymptomesMaladie();
            Object[] objects = objectDAO.getViewOrdersBy(infoSymptomesMaladie, "idMaladie", "asc", "idSymptomes", "asc");
            InfoSymptomesMaladie[] infoSymptomesMaladies = objects.Cast<InfoSymptomesMaladie>().ToArray();
            return infoSymptomesMaladies;
        }

        public InfoSymptomesMaladie[] getInfoByMaladieAndAgeAsc(string idMaladie, int age)
        {
            InfoSymptomesMaladie infoSymptomesMaladie = new InfoSymptomesMaladie();
            Object[] objects = objectDAO.getViewWithExternalWhereOrdersBy(infoSymptomesMaladie, "idMaladie = '"+idMaladie+"' and agemin <= "+age+" and agemax >= "+age, "idMaladie", "asc", "idSymptomes", "asc");
            InfoSymptomesMaladie[] infoSymptomesMaladies = objects.Cast<InfoSymptomesMaladie>().ToArray();
            return infoSymptomesMaladies;
        }

        public InfoSymptomesMaladie[] getInfoBySymptomesAsc(string condition)
        {
            InfoSymptomesMaladie infoSymptomesMaladie = new InfoSymptomesMaladie();
            Object[] objects = objectDAO.getViewWithExternalWhereOrdersBy(infoSymptomesMaladie, condition, "idMaladie", "asc", "idSymptomes", "asc");
            InfoSymptomesMaladie[] infoSymptomesMaladies = objects.Cast<InfoSymptomesMaladie>().ToArray();
            return infoSymptomesMaladies;
        }
    }
}
