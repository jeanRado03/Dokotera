using Dokotera.Models;
using Dokotera.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dokotera.Controllers
{
    [ApiController]
    [Route(("prix/[controller]"))]
    public class PrixController : ControllerBase
    {
        private readonly PrixService _service;
        private readonly PrixMedicamentService _medicamentService;

        public PrixController()
        {
            _medicamentService = new PrixMedicamentService();
            _service = new PrixService();
        }

        [HttpPost("savePrix")]
        public IActionResult savePrix([FromBody] Prix prix)
        {
            _service.savePrix(prix);
            return Ok(prix);
        }

        [HttpPost("savePrixMedicament")]
        public IActionResult savePrixMedicamenet([FromBody] PrixMedicament prixMedicament) 
        {
            _medicamentService.savePrixMedicament(prixMedicament);
            return Ok(prixMedicament);
        }

    }
}
