using Dokotera.Models;
using Dokotera.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dokotera.Controllers
{
    [ApiController]
    [Route(("symptomes/[controller]"))]
    public class SymptomesController : ControllerBase
    {
        private readonly SymptomesService _symptomesService;

        public SymptomesController()
        {
            _symptomesService = new SymptomesService();
        }

        [HttpPost("insertSymptomes")]
        public IActionResult insertSymptomes([FromBody] Symptomes symptomes)
        {
            _symptomesService.saveSymptomes(symptomes);
            return Ok(symptomes);
        }

        [HttpGet("Symptomes")]
        public IEnumerable<Symptomes> GetAllSymptomes()
        {
            return _symptomesService.getAllSymptomes();
        }
    }
}
