using Microsoft.AspNetCore.Mvc;
using BDAS2_Kratky_Horak_Semestralni_Prace.Models;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class MaterialController : Controller
    {

        private readonly OracleDatabaseHelper _connectionString;

        public MaterialController(OracleDatabaseHelper connection)
        {
            _connectionString = connection;
        }

        //Zobrazení seznamu materiálů
        [HttpGet]
        public IActionResult Index()
        {
            var materials = _connectionString.GetAllMaterials();
            return View(materials);
        }

        //Akce pro vytvoření nového materiálu (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Akce pro vytvoření nového materiálu (POST)
        [HttpPost]
        public IActionResult Create(Material material)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddMaterial(material);
                return RedirectToAction("Index");
            }
            return View(material);
        }
    }
}
