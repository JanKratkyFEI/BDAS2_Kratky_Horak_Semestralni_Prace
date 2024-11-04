using BDAS2_Kratky_Horak_Semestralni_Prace.Helpers;
using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class AccountController : Controller
    {
        private readonly OracleDatabaseHelper _connectionString;
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Uživatelské jmeno a heslo jsou povinné.");
                return View();
            }

            //Kontrola zda uživatel s tímto jménem již existuje (použij příslušný mechanismus, např. dotaz na DB)


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
                Jmeno = "Default Jmeno",
                Prijmeni = "Defualt Prijmeni",
            };
            //Uložení nového usera do db
            _connectionString.AddZamestnanec(newZamestnanec);

            //Redirect na stránku přihlášení po úspešné registraci.
            return RedirectToAction("Login");


        }
        //Simulované metody pro kontroli a vytvoření uživatele, pak nahradíme triggerem
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
            // Základní validace přihlášení - zde může být logika ověřování uživatele
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
            string hashedPassword = PasswordHelper.HashPassword(password);
            if (user.Password != hashedPassword)
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
