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
                // Znovu naplnění ViewBag
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(obraz);
            }

            try
            {
                // Vložení obrazu
                _connectionString.AddObraz(obraz);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo k chybě při ukládání obrazu: " + ex.Message);

                // Znovu naplnění ViewBag při výjimce
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(obraz);
            }
        }






        [HttpGet]
        public IActionResult CreateFotografie()
        {
            var stavy = _connectionString.GetAllStavyPredmetu();
            var sbirky = _connectionString.GetAllSbirky();

            if (stavy == null || !stavy.Any())
            {
                ModelState.AddModelError("", "Seznam stavů není dostupný.");
                ViewBag.Stavy = new List<SelectListItem>();
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
                ViewBag.Sbirky = new List<SelectListItem>();
            }
            else
            {
                ViewBag.Sbirky = sbirky.Select(s => new SelectListItem
                {
                    Value = s.IdSbirka.ToString(),
                    Text = s.Nazev
                }).ToList();
            }

            return View(new Fotografie());
        }


        [HttpPost]
        public IActionResult CreateFotografie(Fotografie fotografie)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(fotografie);
            }

            try
            {
                _connectionString.AddFotografie(fotografie);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo k chybě při ukládání fotografie: " + ex.Message);
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(fotografie);
            }
        }

        [HttpGet]
        public IActionResult CreateSocha()
        {
            var stavy = _connectionString.GetAllStavyPredmetu();
            var sbirky = _connectionString.GetAllSbirky();

            if (stavy == null || !stavy.Any())
            {
                ModelState.AddModelError("", "Seznam stavů není dostupný.");
                ViewBag.Stavy = new List<SelectListItem>();
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
                ViewBag.Sbirky = new List<SelectListItem>();
            }
            else
            {
                ViewBag.Sbirky = sbirky.Select(s => new SelectListItem
                {
                    Value = s.IdSbirka.ToString(),
                    Text = s.Nazev
                }).ToList();
            }

            return View(new Socha());
        }


        [HttpPost]
        public IActionResult CreateSocha(Socha socha)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(socha);
            }

            try
            {
                _connectionString.AddSocha(socha);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo k chybě při ukládání sochy: " + ex.Message);
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(socha);
            }
        }



        [HttpGet]
        public IActionResult EditObraz(int id)
        {
            var obraz = _connectionString.GetObrazById(id);

            if (obraz == null)
            {
                return NotFound();
            }

            ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();

            ViewBag.Sbirky = _connectionString.GetAllSbirky()
                .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

            return View(obraz);
        }

        [HttpPost]
        public IActionResult EditObraz(Obraz obraz)
        {
            if (!ModelState.IsValid)
            {
                // Znovu naplníme ViewBag s potřebnými daty (stavy, sbírky)
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();

                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(obraz); // Vrací zpět formulář s validací
            }

            try
            {
               

                // Aktualizace specifických atributů obrazu
                _connectionString.UpdateObraz(obraz);

                return RedirectToAction("Index"); // Přesměrování na seznam
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo k chybě při ukládání změn: " + ex.Message);

                // Znovu naplníme ViewBag při výjimce
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();

                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(obraz);
            }
        }

        [HttpGet]
        public IActionResult EditFotografie(int id)
        {
            // Získání dat fotografie podle ID
            var fotografie = _connectionString.GetFotografieById(id);
            if (fotografie == null)
            {
                return NotFound();
            }

            // Naplnění ViewBag s daty pro výběr
            ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
            ViewBag.Sbirky = _connectionString.GetAllSbirky()
                .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

            return View(fotografie);
        }

        [HttpPost]
        public IActionResult EditFotografie(Fotografie fotografie)
        {
            if (!ModelState.IsValid)
            {

                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Chyba v {state.Key}: {error.ErrorMessage}");
                    }
                }

                // Naplnění ViewBag při validaci
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(fotografie);
            }

            try
            {
                // Aktualizace fotografie v databázi
                _connectionString.UpdateFotografie(fotografie);

                return RedirectToAction("Index"); // Přesměrování na seznam
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo k chybě při ukládání změn: " + ex.Message);

                // Naplnění ViewBag při chybě
                ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                    .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
                ViewBag.Sbirky = _connectionString.GetAllSbirky()
                    .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

                return View(fotografie);
            }
        }


        [HttpGet]
        public IActionResult EditSocha(int id)
        {
            // Získání dat sochy podle ID
            var socha = _connectionString.GetSochaById(id);
            if (socha == null)
            {
                return NotFound();
            }

            // Naplnění ViewBag s daty pro výběr
            ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
            ViewBag.Sbirky = _connectionString.GetAllSbirky()
                .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

            return View(socha);
        }

        //[HttpPost]
        //public IActionResult EditSocha(Socha socha)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var state in ModelState)
        //        {
        //            foreach (var error in state.Value.Errors)
        //            {
        //                Console.WriteLine($"Chyba v {state.Key}: {error.ErrorMessage}");
        //            }
        //        }

        //        // Naplnění ViewBag při validaci
        //        ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
        //            .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
        //        ViewBag.Sbirky = _connectionString.GetAllSbirky()
        //            .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

        //        return View(socha);
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //    {
        //        // Aktualizace sochy v databázi
        //        _connectionString.UpdateSocha(socha);

        //        return RedirectToAction("Index"); // Přesměrování na seznam
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Došlo k chybě při ukládání změn: " + ex.Message);

        //        // Naplnění ViewBag při chybě
        //        ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
        //            .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
        //        ViewBag.Sbirky = _connectionString.GetAllSbirky()
        //            .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

        //        return View(socha);
        //    }

        //    }


        //}

        [HttpPost]
        public IActionResult EditSocha(Socha socha)
        {
            // Naplnění ViewBag před validací, aby bylo dostupné ve všech případech
            ViewBag.Stavy = _connectionString.GetAllStavyPredmetu()
                .Select(s => new SelectListItem { Value = s.IdStav.ToString(), Text = s.Stav }).ToList();
            ViewBag.Sbirky = _connectionString.GetAllSbirky()
                .Select(s => new SelectListItem { Value = s.IdSbirka.ToString(), Text = s.Nazev }).ToList();

            // Validace modelu
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Chyba v {state.Key}: {error.ErrorMessage}");
                    }
                }

                return View(socha);
            }

            try
            {
                // Aktualizace sochy v databázi
                _connectionString.UpdateSocha(socha);

                return RedirectToAction("Index"); // Přesměrování na seznam
            }
            catch (Exception ex)
            {
                // Zpracování výjimky
                ModelState.AddModelError("", "Došlo k chybě při ukládání změn: " + ex.Message);
                return View(socha);
            }
        }







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

        [HttpGet]
        public IActionResult Details(int id)
        {
            // Načtení předmětu z databáze podle ID
            var predmet = _connectionString.GetPredmetById(id);

            if (predmet == null)
            {
                return NotFound(); // Pokud není nalezen
            }

            // Rozhodnutí podle typu předmětu
            if (predmet.Typ == "O")
            {
                var obraz = _connectionString.GetObrazById(id);
                return View("DetailsObraz", obraz);
            }
            else if (predmet.Typ == "F")
            {
                var fotografie = _connectionString.GetFotografieById(id);
                return View("DetailsFotografie", fotografie);
            }
            else if (predmet.Typ == "S")
            {
                var socha = _connectionString.GetSochaById(id);
                return View("DetailsSocha", socha);
            }

            return NotFound(); // Pokud typ není rozpoznán
        }


        [HttpGet]
        public IActionResult NonDisplayedItems()
        {
            var nonDisplayedItems = _connectionString.GetNonDisplayedItems();
            return View(nonDisplayedItems);
        }

    }

}
