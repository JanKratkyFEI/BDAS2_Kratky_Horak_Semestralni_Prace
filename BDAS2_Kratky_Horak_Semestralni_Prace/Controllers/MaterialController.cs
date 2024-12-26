using Microsoft.AspNetCore.Mvc;
using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Authorization;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    
    public class MaterialController : BaseController
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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var material = _connectionString.GetMaterialById(id); // Získat detail předmětu podle ID
            if (material == null)
            {
                return NotFound("Předmět nenalezen.");
            }

            return View(material); // Vrací potvrzovací stránku s detailem předmětu
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _connectionString.DeleteMaterial(id); // Volání metody pro mazání
                return RedirectToAction("Index"); // Přesměrování zpět na seznam předmětů
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Chyba při mazání předmětu: {ex.Message}");
                return RedirectToAction("Delete", new { id = id });
            }
        }


    }
}
