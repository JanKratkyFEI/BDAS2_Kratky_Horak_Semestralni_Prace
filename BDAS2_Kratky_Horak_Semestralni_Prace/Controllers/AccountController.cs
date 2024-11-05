using BDAS2_Kratky_Horak_Semestralni_Prace.Helpers;
using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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
        public IActionResult Activate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Activate(string jmeno, string prijmeni, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(jmeno) || string.IsNullOrWhiteSpace(prijmeni) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Všechna pole jsou povinná.");
                return View();
            }

            //Najde zamestnance dle jmena
            var existingZamestnanec = _connectionString.GetZamestnanecByName(jmeno, prijmeni);

            if (existingZamestnanec == null)
            {
                ModelState.AddModelError("", "Zaměstnanec nebyl nalezen.");
                return View();
            }

            if(!string.IsNullOrEmpty(existingZamestnanec.Username) || !string.IsNullOrEmpty(existingZamestnanec.Password))
            {
                ModelState.AddModelError("", "Účet je již aktivován.");
                return View();
            }

            //zašifrovat heslo a aktualizovat udaje
            existingZamestnanec.Username = username;
            existingZamestnanec.Password = PasswordHelper.HashPassword(password);

            //uložit změny do db
            _connectionString.UpdateZamestnanec(existingZamestnanec);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password, string jmeno, string prijmeni)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Uživatelské jmeno a heslo jsou povinné.");
                return View();
            }

            //Kontrola zda uživatel s tímto jménem již existuje


            var existingUser = _connectionString.GetZamestnanecByUsername(username);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Uživatel s tímto jménem již existuje.");
                return View();

               
            }
            //šifrování
            string hashedPassword = PasswordHelper.HashPassword(password);

            //vytvoření nového zaměstnance s rolí "User"
            var newZamestnanec = new Zamestnanec
            {
                Username = username,
                Password = hashedPassword,
                Jmeno = jmeno,
                Prijmeni = prijmeni,
            };
            //Uložení nového usera do db
            _connectionString.AddZamestnanec(newZamestnanec);

            //Redirect na stránku přihlášení po úspešné registraci.
            return RedirectToAction("Login");


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

        }
    }
