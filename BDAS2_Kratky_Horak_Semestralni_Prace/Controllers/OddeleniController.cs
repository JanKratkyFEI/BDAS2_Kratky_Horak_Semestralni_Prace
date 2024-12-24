using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class OddeleniController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public OddeleniController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var oddeleni = _connectionString.GetAllOddeleni();
            return View(oddeleni);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MuzeumList = _connectionString.GetAllMuzea();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Oddeleni oddeleni)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddOddeleni(oddeleni);
                return RedirectToAction("Index");
            }
            ViewBag.MuzeumList = _connectionString.GetAllMuzea();
            return View(oddeleni);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var oddeleni = _connectionString.GetOddeleniById(id);
            if (oddeleni == null)
            {
                return NotFound();
            }
            ViewBag.MuzeumList = _connectionString.GetAllMuzea();
            return View(oddeleni);
        }

        [HttpPost]
        public IActionResult Edit(Oddeleni oddeleni)
        {
            if (ModelState.IsValid)
            {
                _connectionString.UpdateOddeleni(oddeleni);
                return RedirectToAction("Index");
            }
            ViewBag.MuzeumList = _connectionString.GetAllMuzea();
            return View(oddeleni);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var oddeleni = _connectionString.GetOddeleniById(id);
            if (oddeleni == null)
            {
                return NotFound();
            }
            return View(oddeleni);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _connectionString.DeleteOddeleni(id);
            return RedirectToAction("Index");
        }
    }

}
