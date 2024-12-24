using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{

    public class ObecController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public ObecController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var obce = _connectionString.GetAllObce(); // Získání všech obcí
            return View(obce); // Vrátí seznam obcí do view
        }


        [HttpGet]
        public IActionResult Create()
        {
            // Pokud chcete zobrazit seznam zemí pro výběr
            ViewBag.ZemeList = _connectionString.GetAllZeme();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Obec obec)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddObec(obec);
                return RedirectToAction("Index");
            }

            // Pokud validace selže, znovu načtěte seznam zemí
            ViewBag.ZemeList = _connectionString.GetAllZeme();
            return View(obec);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var obec = _connectionString.GetObecById(id);
            if (obec == null)
            {
                return NotFound();
            }

            

            ViewBag.ZemeList = _connectionString.GetAllZeme();
            return View(obec);
        }

        [HttpPost]
        public IActionResult Edit(Obec obec)
        {
            if (ModelState.IsValid)
            {
                _connectionString.UpdateObec(obec);
                return RedirectToAction("Index");
            }

           

            ViewBag.ZemeList = _connectionString.GetAllZeme();
            return View(obec);
        }

    }
}
