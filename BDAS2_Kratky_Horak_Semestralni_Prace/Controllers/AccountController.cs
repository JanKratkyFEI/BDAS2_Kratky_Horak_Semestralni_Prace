using BDAS2_Kratky_Horak_Semestralni_Prace.Helpers;
using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class AccountController : Controller
    {
        private readonly OracleDatabaseHelper _connectionString;

        public AccountController(OracleDatabaseHelper connectionString)
        {
            _connectionString = connectionString;
        }
        [HttpGet]
        public IActionResult RegisterExistingEmployee()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Register (RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
			if (model.IsExistingEmployee)
			{
				return RedirectToAction("RegisterExistingEmployee", model);
			}
			else
			{
				return RedirectToAction("RegisterNewEmployee", model);
			}

		}

        [HttpPost]
        public IActionResult RegisterNewEmployee(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Register",model);
            }

            var novyZamestnanec = new Zamestnanec
            {
                Jmeno = model.Jmeno,
                Prijmeni = model.Prijmeni,
                Username = model.Username,
                Password = PasswordHelper.HashPassword(model.Password), // Heslo bude hashované
                Email = model.Email,
                Role = "registered", // Výchozí role
                Plat = 20000, // Výchozí plat (nastavit podle aktuální minimální mzdy)
                DatumZamestnani = DateTime.Now, // Datum registrace jako datum zaměstnání
                Pozice = "Nový kolega", // Výchozí hodnota pro pozici
                Telefon = "N/A", // Defaultní hodnota, pokud nemáme ve formuláři
                RodCislo = "N/A", // Defaultní hodnota, pokud není uvedeno
                TypSmlouva = "Plný úvazek", // Výchozí hodnota
                IdAdresa = 1, // Výchozí hodnota
                IdOddeleni = 1 // Výchozí hodnota
            };
            try
            {
                _connectionString.InsertZamestnanec(novyZamestnanec);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Chyba při registraci: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

                ModelState.AddModelError("", "Registrace se nezdařila. Zkuste to prosím znovu.");
                return View("Register", model);
            }

            
        }

      
        


        //Simulované metody pro kontrolu a vytvoření uživatele, pak nahradíme triggerem
        private bool CheckUserExists(string username)
        {
            //Zkontroluj db, zda user existuje TODO

            return false;

        }

        private void CreateUser(string username, string password, string role)
        {
            //Přidej nového uživatele do db
        }


        [HttpGet]
            public IActionResult Login()
            {
                // Vrátíme prázdný formulář pro přihlášení
                return View();
            }

            [HttpPost]
            public IActionResult Login(string username, string password)
            {
            // Základní validace přihlášení
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Uživatelské jméno a heslo jsou povinné.");
                return View();
            }
            //načtení usera z db dle username
            var user = _connectionString.GetZamestnanecByUsername(username);
            if (user == null)
            {
                ModelState.AddModelError("", "Neplatné přihlašovací údaje.");
                return View();
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError("", "Neplatné přihlašovací údaje.");
                return View();
            }
            // Ověření hesla - porovnání hash hesla
            if (!PasswordHelper.VerifyPassword(password, user.Password))
            {
                ModelState.AddModelError("", "Neplatné přihlašovací údaje.");
                return View();
            }
            
            // Nastavení session pro přihlášení
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Username", username);

            // Přesměrování na hlavní stránku po úspěšném přihlášení
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public IActionResult Logout()
        {
            //vyčistí session při odhlášení
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var zamestnanec = _connectionString.GetZamestnanecJoinDetails(username);
            if (zamestnanec == null)
            {
                return NotFound("Zaměstnanec nebyl nalezen.");
            }
           

            return View(zamestnanec);
        }

        [HttpGet]
        public IActionResult EmployeeProfile(int id)
        {
            var zamestnanec = _connectionString.GetZamestnanecFromView(id);
            if (zamestnanec == null)
            {
                return NotFound();
            }

            return View(zamestnanec);
        }

        [HttpGet]
        public IActionResult Search(string searchQuery)
        {
            var employees = _connectionString.SearchZamestnanci(searchQuery ?? string.Empty);

            return View(employees);
        }

    }
    }
