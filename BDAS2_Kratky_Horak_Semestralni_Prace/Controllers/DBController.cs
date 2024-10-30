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
            List<Predmet> predmety = _dbHelper.GetPredmety();

            // Vrátíme data do view nebo je zobrazíme přímo
            return View(predmety);
        }
    }
}

