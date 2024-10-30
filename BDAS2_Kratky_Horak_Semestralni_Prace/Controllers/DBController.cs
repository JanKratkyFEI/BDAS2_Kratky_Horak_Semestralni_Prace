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
        private readonly OracleDatabaseHelper _dbHelper;

        public DBController(IConfiguration configuration)
        {
            // Inicializace OracleDatabaseHelper s connection stringem z konfigurace
            string connectionString = configuration.GetConnectionString("OracleDbConnection");
            _dbHelper = new OracleDatabaseHelper(connectionString);
        }

        public IActionResult TestConnection()
        {
            List<Autor> autori = _dbHelper.GetAutori();

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
            var predmety = // Nacti zaznamy z db
                return View(predmety);
        }
		// Details: Zobrazí detail jednoho záznamu
		public IActionResult Details(int id)
		{
			var predmet = // Načti záznam podle ID

		if (predmet == null) return NotFound();
			return View(predmet);
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
			var predmet = // Načti záznam podle ID

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

