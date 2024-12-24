using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{


    public class SbirkaController : BaseController
    {

        private readonly OracleDatabaseHelper _connectionString;
        public SbirkaController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sbirky = _connectionString.GetAllSbirky();
            return View(sbirky);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Muzea = _connectionString.GetAllMuzea();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Sbirka sbirka)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddSbirka(sbirka);
                return RedirectToAction("Index");
            }

            ViewBag.Muzea = _connectionString.GetAllMuzea();
            return View(sbirka);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sbirka = _connectionString.GetSbirkaById(id);
            if (sbirka == null)
            {
                return NotFound();
            }

            ViewBag.Muzea = _connectionString.GetAllMuzea();
            return View(sbirka);
        }

        [HttpPost]
        public IActionResult Edit(Sbirka sbirka)
        {
            ModelState.Remove("MuzeumNazev");

            if (ModelState.IsValid)
            {
                _connectionString.UpdateSbirka(sbirka);
                return RedirectToAction("Index");
            }

            ViewBag.Muzea = _connectionString.GetAllMuzea();
            return View(sbirka);
        }

    }
}
