using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

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
           ViewBag.Stavy = new SelectList(new List<string> { "uskladněno", "vystavěno", "vypůjčeno" });
             
            return View();
        }

        [HttpPost]
        public IActionResult Create(StavPredmetu stav)
        {
            
            if (GetCurrentRole() != "Admin")
            {
                return Forbid();
            }

           
            System.Diagnostics.Debug.WriteLine($"ViewBag.Stavy: {string.Join(", ", (ViewBag.Stavy as SelectList).Select(x => x.Text))}");
            System.Diagnostics.Debug.WriteLine($"Stav: {stav.Stav}");
            System.Diagnostics.Debug.WriteLine($"Začátek: {stav.ZacatekStav}");
            System.Diagnostics.Debug.WriteLine($"Konec: {stav.KonecStav}");

            if (!ModelState.IsValid)
            {
                
                ViewBag.Stavy = new SelectList(new List<string> { "uskladněno", "vystavěno", "vypůjčeno" });
               

                return View(stav);
            }

            if (ModelState.IsValid)
            {

                if (stav.Stav == "vypůjčeno" && (stav.KonecStav - stav.ZacatekStav).TotalDays > 90)
                {
                    ModelState.AddModelError("", "Doba vypůjčení nesmí být delší než 3 měsíce.");
                    ViewBag.Stavy = new SelectList(new List<string> { "uskladněno", "vystavěno", "vypůjčeno" });

                    return View(stav);
                }
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
            ViewBag.Stavy = new SelectList(new List<string> { "uskladněno", "vystavěno", "vypůjčeno" });
            return View(stav);
        }

        [HttpPost]
        public IActionResult Edit(StavPredmetu stav)
        {
            if (GetCurrentRole() != "Admin")
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Stavy = new SelectList(new List<string> { "uskladněno", "vystavěno", "vypůjčeno" });

                return View(stav);
            }
            try
            {
                _connectionString.UpdateStavPredmetu(stav);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Chyba při úpravě stavu: {ex.Message}");
                ViewBag.Stavy = new SelectList(new List<string> { "uskladněno", "vystavěno", "vypůjčeno" });
                return View(stav);
            }


           
            
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
