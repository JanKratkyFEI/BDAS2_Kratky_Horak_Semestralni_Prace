using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class AdresaController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public AdresaController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var adresy = _connectionString.GetAllAdresa();
            return View(adresy);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Obce = _connectionString.GetAllObce();
            ViewBag.Muzea = _connectionString.GetAllMuzea();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Adresa adresa)
        {
            if (ModelState.IsValid)
            {
                _connectionString.AddAdresa(adresa);
                return RedirectToAction("Index");
            }

            ViewBag.Obce = _connectionString.GetAllObce();
            ViewBag.Muzea = _connectionString.GetAllMuzea();
            return View(adresa);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var adresa = _connectionString.GetAdresaById(id);
            if (adresa == null)
            {
                return NotFound();
            }

            var viewModel = new AdresaEditViewModel
            {
                Adresa = adresa,
                Obce = _connectionString.GetAllObce().Select(o => new SelectListItem
                {
                    Value = o.IdObec.ToString(),
                    Text = o.Nazev
                }),
                Muzea = _connectionString.GetAllMuzea().Select(m => new SelectListItem
                {
                    Value = m.IdMuzeum.ToString(),
                    Text = m.Nazev
                })
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Edit(AdresaEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    var key = modelState.Key;
                    var errors = modelState.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                // Pokud je model neplatný, znovu načtěte potřebná data do ViewBag
                model.Obce = _connectionString.GetAllObce().Select(o => new SelectListItem
                {
                    Value = o.IdObec.ToString(),
                    Text = o.Nazev
                });
                model.Muzea = _connectionString.GetAllMuzea().Select(m => new SelectListItem
                {
                    Value = m.IdMuzeum.ToString(),
                    Text = m.Nazev
                });

                // Vraťte uživatele zpět na view s chybami
                return View(model);
            }

            // Získejte `Adresa` objekt z modelu
            var adresa = model.Adresa;

            // Aktualizujte data pomocí DB helperu
            _connectionString.UpdateAdresa(adresa);

            // Přesměrujte zpět na seznam adres
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete (int id)
        {
            var predmet = _connectionString.GetAdresaById(id);
            if (predmet == null)
            {
                return NotFound("Předmět nalezen.");
            }

            return View(predmet);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _connectionString.DeleteAdresa(id);
                return RedirectToAction("Index");
            }
            catch( Exception ex)
            {
                ModelState.AddModelError("", $"Chyba při mazání adres: {ex.Message}");
                return RedirectToAction("Delete", new { id = id });
            }
        }
    }
}

