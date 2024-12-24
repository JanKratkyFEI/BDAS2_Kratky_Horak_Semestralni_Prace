using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class AutorController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public AutorController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var autori = _connectionString.GetAllAutori();
            return View(autori);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Autor autor)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddAutor(autor);
                return RedirectToAction("Index");
            }
            return View(autor);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var autor = _connectionString.GetAutorById(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        [HttpPost]
        public IActionResult Edit(Autor autor)
        {
            if (ModelState.IsValid)
            {
                _connectionString.UpdateAutor(autor);
                return RedirectToAction("Index");
            }
            return View(autor);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var autor = _connectionString.GetAutorById(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _connectionString.DeleteAutor(id);
            return RedirectToAction("Index");
        }
    }

}
