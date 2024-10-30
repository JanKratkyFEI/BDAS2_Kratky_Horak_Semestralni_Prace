using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class AccountController : Controller
    {
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


            bool userExists = CheckUserExists(username);
            if (userExists)
            {
                ModelState.AddModelError("", "Uživatel s tímto jménem již existuje.");
                return View();

               
            }
            //stvoříme nového uživatele s rolí "User"
            CreateUser(username, password, "User");

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
                if (username == "admin" && password == "password") // Změň na reálné ověřování
                {
                // Nastavení session nebo cookies pro přihlášení
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetString("Username", username);

                    return RedirectToAction("Index", "Home"); // Přesměrování po úspěšném přihlášení
                }
                ModelState.AddModelError("", "Neplatné přihlašovací údaje");
                return View();
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
