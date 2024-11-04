using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Configuration;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class DBController : Controller
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

        public IActionResult Create(string typ)
        {
            ViewData["Typ"] = typ;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                if (predmet.Typ == "Fotografie")
                {

                }
                else if (predmet.Typ == "Obraz")
                {

                }
                else if (predmet.Typ == "Socha")
                {

                }
                return RedirectToAction("Index");
            }
            return View(predmet);
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

        // Edit: Zobrazení formuláře pro úpravu záznamu
        public IActionResult Edit(int id)
		{
			var predmet = // Načti záznam podle ID

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
		public IActionResult Delete(int id)
		{
            var predmet = ; // Načti záznam podle ID TODO

		if (predmet == null) return NotFound();
			return View(predmet);
		}

		// Delete (POST): Smazání záznamu
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(int id)
		{
			// Smaž záznam z databáze
			return RedirectToAction("Index");
		}

	}
}

