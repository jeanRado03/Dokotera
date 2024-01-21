using Dokotera.Services;
using Dokotera.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dokotera.Controllers
{
    [ApiController]
    [Route(("maladie/[controller]"))]
    public class MaladieController : ControllerBase
    {
        private readonly MaladieService _maladieService;

        public MaladieController()
        {
            _maladieService = new MaladieService();
        }

        [HttpPost("insertMaladie")]
        public IActionResult insertMaladie([FromBody] Maladie maladie)
        {
            _maladieService.saveMaladie(maladie);
            return Ok(maladie);
        }

        [HttpGet("Maladies")]
        public IEnumerable<Maladie> listMaladies()
        {
            return _maladieService.getAllMaladie();
        }
    }
}
