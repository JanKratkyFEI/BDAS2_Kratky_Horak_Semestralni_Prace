using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class ZemeController : Controller
    {
        private readonly OracleDatabaseHelper _connectionString;

        public ZemeController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var zeme = _connectionString.GetAllZeme();
            return View(zeme);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Zeme zeme)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddZeme(zeme);
                return RedirectToAction("Index");
            }
            return View(zeme);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var zeme = _connectionString.GetZemeById(id);
            if (zeme == null)
            {
                return NotFound();
            }
            return View(zeme);
        }

        [HttpPost]
        public IActionResult Edit(Zeme zeme)
        {
            if (ModelState.IsValid)
            {
                _connectionString.UpdateZeme(zeme);
                return RedirectToAction("Index");
            }
            return View(zeme);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var zeme = _connectionString.GetZemeById(id);
            if (zeme == null)
            {
                return NotFound();
            }
            return View(zeme);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _connectionString.DeleteZeme(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                ViewBag.ErrorMessage = "Chyba při mazání záznamu: " + ex.Message;
                return View("Error");
            }
        }




    }
}
