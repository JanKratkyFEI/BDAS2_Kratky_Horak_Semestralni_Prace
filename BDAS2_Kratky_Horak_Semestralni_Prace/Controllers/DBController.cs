using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class DBController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public DBController(IConfiguration configuration)
        {
            // Inicializace OracleDatabaseHelper s connection stringem z konfigurace
            string connectionString = configuration.GetConnectionString("OracleDbConnection");
             _connectionString = new OracleDatabaseHelper(connectionString);
            
            
        }

        public IActionResult TestConnection()
        {
            List<Autor> autori = _connectionString.GetAutori();

            // Vrátíme data do view nebo je zobrazíme přímo
            return View(autori);
        }

        public IActionResult Browse(string section)
        {
            ViewData["Section"] = section;
            //Načti data podle sekce
            return View();
        }

        public IActionResult CreatePredmet(string typ)
        {
            ViewData["Typ"] = typ;
            return View(new PredmetViewModel { Typ = typ });
        }

        [HttpPost]
        public IActionResult CreatePredmet(PredmetViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Rozhodněte se podle typu předmětu (Fotografie, Obraz, Socha)
                switch (model.Typ)
                {
                    case "Fotografie":
                        var fotografie = new Fotografie
                        {
                            Nazev = model.Nazev,
                            Popis = model.Popis,
                            Zanr = model.Zanr,
                            Licence = model.Licence
                        };
                        _connectionString.AddFotografie(fotografie);
                        break;

                    case "Obraz":
                        var obraz = new Obraz
                        {
                            Nazev = model.Nazev,
                            Popis = model.Popis,
                            UmeleckyStyl = model.UmeleckyStyl,
                            Medium = model.Medium
                        };
                        _connectionString.AddObraz(obraz);
                        break;

                    case "Socha":
                        var socha = new Socha
                        {
                            Nazev = model.Nazev,
                            Popis = model.Popis,
                            Vaha = model.Vaha,
                            TechnikaTvorby = model.TechnikaTvorby
                        };
                        _connectionString.AddSocha(socha);
                        break;

                    default:
                        ModelState.AddModelError("", "Neplatný typ předmětu.");
                        return View(model);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Index()
        {
            var predmety = _connectionString.GetPredmety();
            
                return View(predmety);
        }
        public IActionResult IndexAutor()
        {
            var autori = _connectionString.GetAutori();
            return View(autori);
        }
        public IActionResult IndexAdresa()
        {
            var adresy = _connectionString.GetAdresy();
            return View(adresy);
        }

        public IActionResult DetailsAdresa(int id)
        {
            var adresa = _connectionString.GetAdresaById(id);
            if (adresa == null) return NotFound();
            return View(adresa);
        }


        // Details: Zobrazí detail jednoho záznamu
        public IActionResult Details(int id)
		{
            var typ = _connectionString.GetPredmetTypeById(id); // Načti záznam podle ID

		if (typ == null) return NotFound();
			//return View(predmet);
            switch(typ)
            {
                case "Obraz":
                    var obraz = _connectionString.GetObrazById(id);
                    return View("DetailsObraz", obraz);
                case "Fotografie":
                    var fotografie = _connectionString.GetFotografieById(id);
                    return View("DetailsFotografie", fotografie);
                case "Socha":
                    var socha = _connectionString.GetSochaById(id);
                    return View("DetailsSocha", socha);
                default:
                    return NotFound();
            }
		}
        public IActionResult DetailsAutor(int id)
        {
            var autor = _connectionString.GetAutorById(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        // Edit: Zobrazení formuláře pro úpravu záznamu REMEMBER: MRKNOUT NA PARTIAL VIEWS
        public IActionResult Edit(int id)
		{
            var predmet = _connectionString.GetPredmetTypeById(id);// Načti záznam podle ID

		if (predmet == null) return NotFound();
			return View(predmet);
		}

		// Edit (POST): Aktualizace existujícího záznamu
		[HttpPost]
		public IActionResult Edit(int id, Predmet predmet)
		{
			if (ModelState.IsValid)
			{
				// Aktualizuj záznam v databázi
				return RedirectToAction("Index");
			}
			return View(predmet);
		}

		// Delete: Zobrazení potvrzovací stránky pro smazání
		//public IActionResult Delete(int id)
		//{
  //          var predmet = _connectionString.De; // Načti záznam podle ID TODO

		//if (predmet == null) return NotFound();
		//	return View(predmet);
		//}

		// Delete (POST): Smazání záznamu
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(int id)
		{
			// Smaž záznam z databáze
			return RedirectToAction("Index");
		}


        public IActionResult ShowDatabaseObjects()
        {
            var tables = _connectionString.GetUserTables();
            var triggers = _connectionString.GetUserTriggers();

            var model = new DatabaseObjectsViewModel
            {
                Tables = tables,
                Triggers = triggers
            };

            return View(model);
        }


        

        [HttpGet]
        public IActionResult SearchEmploy(string searchQuery)
        {
            var zamestnanci = _connectionString.SearchEmployeees(searchQuery);
            return View(zamestnanci);
        }

        

    }
}

