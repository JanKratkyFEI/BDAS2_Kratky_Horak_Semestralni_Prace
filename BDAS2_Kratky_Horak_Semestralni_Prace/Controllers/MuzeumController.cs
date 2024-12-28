using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class MuzeumController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public MuzeumController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var muzea = _connectionString.GetAllMuzea();
            return View(muzea);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Muzeum muzeum)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddMuzeum(muzeum);
                return RedirectToAction("Index");
            }
            return View(muzeum);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var muzeum = _connectionString.GetMuzeumById(id);
            if (muzeum == null)
            {
                return NotFound();
            }
            return View(muzeum);
        }

        [HttpPost]
        public IActionResult Edit(Muzeum muzeum)
        {
            if (ModelState.IsValid)
            {
                _connectionString.UpdateMuzeum(muzeum);
                return RedirectToAction("Index");
            }
            return View(muzeum);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var muzeum = _connectionString.GetMuzeumById(id);
            if (muzeum == null)
            {
                return NotFound();
            }
            return View(muzeum);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _connectionString.DeleteMuzeum(id);
            return RedirectToAction("Index");
        }
    }

}
