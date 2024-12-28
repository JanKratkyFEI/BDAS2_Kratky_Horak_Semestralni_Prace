using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class StavPredmetuController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public StavPredmetuController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var stavy = _connectionString.GetAllStavyPredmetu();
            return View(stavy);
        }

        [HttpGet]
        public IActionResult Create()
        {
           
             
            return View();
        }

        [HttpPost]
        public IActionResult Create(StavPredmetu stav)
        {
            if (GetCurrentRole() != "Admin")
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                _connectionString.AddStavPredmetu(stav);
                return RedirectToAction("Index");
            }
            return View(stav);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            if (GetCurrentRole() != "Admin")
            {
                return Forbid();
            }
            var stav = _connectionString.GetStavPredmetuById(id);
            if (stav == null)
            {
                return NotFound();
            }
            return View(stav);
        }

        [HttpPost]
        public IActionResult Edit(StavPredmetu stav)
        {
            if (GetCurrentRole() != "Admin")
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                _connectionString.UpdateStavPredmetu(stav);
                return RedirectToAction("Index");
            }
            return View(stav);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (GetCurrentRole() != "Admin")
            {
                return Forbid();
            }


            var stav = _connectionString.GetStavPredmetuById(id);
            if (stav == null)
            {
                return NotFound();
            }
            return View(stav);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (GetCurrentRole() != "Admin")
            {
                return Forbid();
            }
            _connectionString.DeleteStavPredmetu(id);
            return RedirectToAction("Index");
        }
    }

}
