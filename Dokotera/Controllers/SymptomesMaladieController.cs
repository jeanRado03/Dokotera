using Dokotera.Models;
using Dokotera.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.ML.Transforms;
using System.Runtime.InteropServices;
using System.Text;

namespace Dokotera.Controllers
{
    [ApiController]
    [Route(("symptomesmaladie/[controller]"))]
    public class SymptomesMaladieController : ControllerBase
    {
        private readonly SymptomesMaladieService _service;
        private readonly InfoSymptomesMaladieService _infoSymptomesMaladieService;

        public SymptomesMaladieController()
        {
            _service = new SymptomesMaladieService();
            _infoSymptomesMaladieService = new InfoSymptomesMaladieService();
        }

        [HttpPost("insertSymptomesMaladie")]
        public IActionResult insertSymptomesMaladie([FromBody] SymptomesMaladie[] symptomesMaladies)
        {
            _service.saveObjectsSymptomesMaladie(symptomesMaladies);
            return Ok(symptomesMaladies);
        }

        [HttpGet("symptomesMaladie")]
        public IEnumerable<SymptomesMaladie> GetSymptomesMaladies()
        {
            return _service.getAllSymptomesMaladie();
        }

        /*[HttpGet("infoSymptomesMaladie")]
        public Dictionary<string,List<InfoSymptomesMaladie>> GetInfoSymptomesMaladies()
        {
            InfoSymptomesMaladie infoSymptomesMaladie = new InfoSymptomesMaladie();
            return infoSymptomesMaladie.donnees(_infoSymptomesMaladieService);
            //return _infoSymptomesMaladieService.getInfoAsc();
        }*/

        [HttpPost("choixSymptomesPatient")]
        public string[] getInfo([FromBody] SymptomesPattient symptomesPattient)
        {
            Dictionary<string, List<InfoSymptomesMaladie>> dict = symptomesPattient.getListesMaladies();
            List<string> cles = dict.Keys.ToList();
            return cles.ToArray();
        }

        [HttpPost("testSymptomesPatient")]
        public Dictionary<string, List<InfoSymptomesMaladie>>  getTest([FromBody] SymptomesPattient symptomesPattient)
        {
            Dictionary<string, List<InfoSymptomesMaladie>> dict = symptomesPattient.getTest();
            return dict;
        }

        [HttpPost("stringSymptomesPatient")]
        public string getScript([FromBody] SymptomesPattient symptomesPattient)
        {
            return symptomesPattient.getSymptomesPatient();
        }

        [HttpPost("testResultat")]
        public Resultat getResult([FromBody] SymptomesPattient symptomesPattient)
        {
            Resultat resultat = new Resultat();
            resultat.maladiesPatient(symptomesPattient);
            return resultat;
        }
    }
}
