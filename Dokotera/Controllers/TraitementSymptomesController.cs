using Dokotera.Models;
using Dokotera.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dokotera.Controllers
{
    [ApiController]
    [Route(("traitement/[controller]"))]
    public class TraitementSymptomesController : ControllerBase
    {
        private readonly TraitementSymptomesService _traitementSymptomesService;

        public TraitementSymptomesController()
        {
            _traitementSymptomesService = new TraitementSymptomesService();
        }

        [HttpGet("traitementsAllSymptomes")]
        public IEnumerable<TraitementSymptomes> getAllTraitement()
        {
            return _traitementSymptomesService.getAllTraitementSymptomes();
        }

        [HttpGet("testSimplex")]
        public IActionResult actionResult()
        {

            /*(Simplex solver = new Simplex()
            {
                variable = new string[] { "MED1", "MED2", "MED3", "MED4", "X1", "X2", "X3", "X4", "X45, "A1", "A2", "A3", "A4", "A5", "Z" },
                Z = new double[16] { -75, -75, -125, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                inequation = new double[5][] { new double[] { 1, 0, 1, 0, -1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 7 },
                                                new double[] { 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                new double[] { 0, 0, 0, 2, 0, 0, -1, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                                                new double[] { 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1, 0, 0, 0 },
                                                new double[] { 0, 1, 1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1, 0, 9 } },
                sommeArtificielle = new double[16] { -1, -1, -2, -2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, -16 },
            };*/
            /*Simplex solver = new Simplex()
            {
                variable = new string[] { "MED1", "MED2", "MED3", "MED4", "E1", "E2", "E3", "E4", "E5", "A1", "A2", "Z" },
                Z = new double[] { -75, -75, -125, -100, 0, 0, 0, 0, 0, 0, 0, 0 },
                inequation = new double[5][] { new double[] { 1, 0, 1, 0, -1, 0, 0, 0, 0, 1, 0, 7 },
                                                new double[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                                                new double[] { 0, 0, 0, -2, 0, 0, 1, 0, 0, 0, 0, 0 },
                                                new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                                                new double[] { 0, 1, 1, 0, 0, 0, 0, 0, -1, 0, 1, 9 } },
                sommeArtificielle = new double[] { -1, -1, -2, 0, 1, 0, 0, 0, 1, 0, 0, -16 },
            };*/
            Simplex solver = new Simplex()
            {
                variable = new string[] { "MED1", "MED2", "MED3", "MED5", "MED6", "MED7", "MED8", "MED9", "X1", "X2", "A1", "A2", "X3", "Z" },
                Z = new double[] { -24250, -34400, -33200, -15800, -12250, -18200, -39600, -13000, 0, 0, 0, 0, 0, 0 },
                inequation = new double[][] { new double[] { 5  ,  6  ,  6  ,  0  ,  0  ,  0  ,  0  ,  0  ,  -1  ,  0  ,  0  ,  1  ,  0  ,  7 },
                                                new double[] { 0  ,  0  ,  0  ,  4  ,  3  ,  4  ,  6  ,  0  ,  0  ,  -1  ,  0  ,  0  ,  1  ,  6 },
                                                new double[] { 3  ,  0  ,  0  ,  0  ,  0  ,  0  ,  0  ,  -3  ,  0  ,  0  ,  1  ,  0  ,  0  ,  0 }},
                sommeArtificielle = new double[] { -5 , -6 , -6 , -4 , -3 , -4 , -6 , 0 , 1 , 1 , 0 , 0, 0, -13 },
            };
            double[][] matrice1 = solver.matriceDeuxPhase();
            double[][] matrice2 = solver.algoSimplexedeuxphases(matrice1);
            double[][] matrice3 = solver.withoutArtificial(matrice2);
            double[][] matrice4 = solver.algoSimplexeMin(matrice3);
            Console.WriteLine("Minimiser + somme de variable artificiel");
            for (int i = 0; i < matrice2.Length; i++)
            {
                Console.Write(solver.baseDeuxPhases[i] + "  |  ");
                for (int j = 0; j < matrice2[i].Length; j++)
                {
                    Console.Write(matrice2[i][j] + "  |  ");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine("Minimiser sans variable artificiel");
            for (int i = 0; i < matrice3.Length; i++)
            {
                Console.Write(solver.bazy[i] + "  |  ");
                for (int j = 0; j < matrice3[i].Length; j++)
                {
                    Console.Write(matrice3[i][j] + "  |  ");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine("Resultat Final");
            for (int i = 0; i < matrice4.Length; i++)
            {
                Console.Write(solver.bazy[i] + "  |  ");
                for (int j = 0; j < matrice4[i].Length; j++)
                {
                    Console.Write(matrice4[i][j] + "  |  ");
                }
                Console.WriteLine(" ");
            }
            return Ok();
        }
    }
}
