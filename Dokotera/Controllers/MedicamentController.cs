using Dokotera.Models;
using Dokotera.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dokotera.Controllers
{
    [ApiController]
    [Route(("medicament/[controller]"))]
    public class MedicamentController : ControllerBase
    {
        private readonly MedicamentService _medicamentService;
        private readonly MedicamentSymptomesService _medicamentSymptomesService;
        
        public MedicamentController()
        {
            _medicamentService = new MedicamentService();
            _medicamentSymptomesService = new MedicamentSymptomesService();
        }

        [HttpPost("saveMedicamenet")]
        public IActionResult saveMedicament([FromBody] Medicament medicament)
        {
            _medicamentService.saveMedicament(medicament);
            return Ok(medicament);
        }

        [HttpPost("saveMedicamentSymptomes")]
        public IActionResult saveMedicamentSymptomes([FromBody] MedicamentSymptomes medicamentSymptomes)
        {
            _medicamentSymptomesService.saveMedicamentSymptomes(medicamentSymptomes);
            return Ok(medicamentSymptomes);
        }

    }
}
