using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class PredmetController : Controller
    {
        private readonly OracleDatabaseHelper _connectionString;

        public PredmetController(OracleDatabaseHelper dbHelper)
        {
            _connectionString = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var predmety = _connectionString.GetPredmety();
            foreach (var predmet in predmety)
            {
                Console.WriteLine($"ID: {predmet.IdPredmet}, Nazev: {predmet.Nazev}, StavNazev: {predmet.StavNazev}, SbirkaNazev: {predmet.SbirkaNazev}");
            }

            return View(predmety);
        }
        [HttpGet]
        public IActionResult CreateObraz()
        {
            // Získání seznamu stavů a sbírek z databáze
            var stavy = _connectionString.GetAllStavyPredmetu();
            var sbirky = _connectionString.GetAllSbirky();

            // Kontrola, zda jsou seznamy null nebo prázdné
            if (stavy == null || !stavy.Any())
            {
                ModelState.AddModelError("", "Seznam stavů není dostupný.");
                ViewBag.Stavy = new List<SelectListItem>(); // Nastavení prázdného seznamu, aby to ve view nepadalo
            }
            else
            {
                ViewBag.Stavy = stavy.Select(s => new SelectListItem
                {
                    Value = s.IdStav.ToString(),
                    Text = s.Stav
                }).ToList();
            }

            if (sbirky == null || !sbirky.Any())
            {
                ModelState.AddModelError("", "Seznam sbírek není dostupný.");
                ViewBag.Sbirky = new List<SelectListItem>(); // Nastavení prázdného seznamu, aby to ve view nepadalo
            }
            else
            {
                ViewBag.Sbirky = sbirky.Select(s => new SelectListItem
                {
                    Value = s.IdSbirka.ToString(),
                    Text = s.Nazev
                }).ToList();
            }

            return View(new Obraz());
        }



        [HttpPost]
        public IActionResult CreateObraz(Obraz obraz)
        {
            if (!ModelState.IsValid)
            {
                // Výpis chyb do konzole (pro debugging)
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"Klíč: {state.Key}, Chyby: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }

                // Znovu naplnění ViewBag pro View
                ViewBag.Stavy = new SelectList(_connectionString.GetAllStavyPredmetu(), "IdStav", "Nazev");
                ViewBag.Sbirky = new SelectList(_connectionString.GetAllSbirky(), "IdSbirka", "Nazev");

                return View(obraz);
            }

            // Použití AddObraz k vložení dat
            _connectionString.AddObraz(obraz);

            // Přesměrování na Index
            return RedirectToAction("Index");
        }





        [HttpGet]
        public IActionResult CreateFotografie()
        {
            ViewBag.Stavy = new SelectList(_connectionString.GetAllStavyPredmetu(), "IdStav", "Nazev");
            ViewBag.Sbirky = new SelectList(_connectionString.GetAllSbirky(), "IdSbirka", "Nazev");

            return View(new Fotografie());
        }

        [HttpPost]
        public IActionResult CreateFotografie(Fotografie fotografie)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var errors = state.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Klíč: {state.Key} - Chyba: {error.ErrorMessage}");
                    }
                }
                return View(fotografie);
            }

            var predmet = new Predmet
            {
                Nazev = fotografie.Nazev,
                Stari = fotografie.Stari,
                Popis = fotografie.Popis,
                Typ = "F",
                IdStav = fotografie.IdStav,
                IdSbirka = fotografie.IdSbirka
            };
            _connectionString.InsertPredmet(predmet);

            fotografie.IdPredmet = predmet.IdPredmet;
            _connectionString.AddFotografie(fotografie);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateSocha()
        {
            ViewBag.Stavy = new SelectList(_connectionString.GetAllStavyPredmetu(), "IdStav", "Nazev");
            ViewBag.Sbirky = new SelectList(_connectionString.GetAllSbirky(), "IdSbirka", "Nazev");

            return View(new Socha());
        }

        [HttpPost]
        public IActionResult CreateSocha(Socha socha)
        {
            if (!ModelState.IsValid)
                return View(socha);

            var predmet = new Predmet
            {
                Nazev = socha.Nazev,
                Stari = socha.Stari,
                Popis = socha.Popis,
                Typ = "S",
                IdStav = socha.IdStav,
                IdSbirka = socha.IdSbirka
            };
            _connectionString.InsertPredmet(predmet);

            socha.IdPredmet = predmet.IdPredmet;
            _connectionString.AddSocha(socha);

            return RedirectToAction("Index");
        }




        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public IActionResult Create(Predmet predmet, Obraz obraz, Fotografie fotografie, Socha socha)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Diagnostický výpis chyb
        //        foreach (var key in ModelState.Keys)
        //        {
        //            var state = ModelState[key];
        //            if (state.Errors.Any())
        //            {
        //                Console.WriteLine($"Klíč: {key}");
        //                foreach (var error in state.Errors)
        //                {
        //                    Console.WriteLine($"  - Chyba: {error.ErrorMessage}");
        //                }
        //            }
        //        }

        //        return View(predmet);
        //    }

        //    _connectionString.InsertPredmet(predmet);

        //    switch (predmet.Typ)
        //    {
        //        case "Obraz":
        //            obraz.IdPredmet = predmet.IdPredmet;
        //            _connectionString.AddObraz(obraz);
        //            break;
        //        case "Fotografie":
        //            fotografie.IdPredmet = predmet.IdPredmet;
        //            _connectionString.AddFotografie(fotografie);
        //            break;
        //        case "Socha":
        //            socha.IdPredmet = predmet.IdPredmet;
        //            _connectionString.AddSocha(socha);
        //            break;
        //    }

        //    return RedirectToAction("Index");
        //}



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var predmet = _connectionString.GetPredmetById(id);
            if (predmet == null)
                return NotFound();

            return View(predmet);
        }

        [HttpPost]
        public IActionResult Edit(Predmet predmet, Obraz obraz, Fotografie fotografie, Socha socha)
        {
            if (!ModelState.IsValid)
                return View(predmet);

            _connectionString.UpdatePredmet(predmet);

            switch (predmet.Typ)
            {
                case "Obraz":
                    //_connectionString.UpdateObraz(obraz);
                    _connectionString.UpdatePredmet(obraz);
                    break;
                case "Fotografie":
                    //_connectionString.UpdateFotografie(fotografie);
                    _connectionString.UpdatePredmet(fotografie);
                    break;
                case "Socha":
                    // _connectionString.UpdateSocha(socha);
                    _connectionString.UpdatePredmet(socha);
                    break;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var predmet = _connectionString.GetPredmetById(id); // Získat detail předmětu podle ID
            if (predmet == null)
            {
                return NotFound("Předmět nenalezen.");
            }

            return View(predmet); // Vrací potvrzovací stránku s detailem předmětu
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id, string typ)
        {
            try
            {
                _connectionString.DeletePredmet(id, typ); // Volání metody pro mazání
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
