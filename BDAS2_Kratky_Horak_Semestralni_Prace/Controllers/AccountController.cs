using BDAS2_Kratky_Horak_Semestralni_Prace.Helpers;
using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class AccountController : BaseController
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

        // <E> <M> <U> <L> <A> <C> <E>
        [HttpPost]
        public IActionResult EmulateRole(string emulateRole)
        {
            var originalRole = HttpContext.Session.GetString("Role");
            if (originalRole == "Admin")
            {
                // Nastaví emulovanou roli do session
                HttpContext.Session.SetString("EmulatedRole", emulateRole);
            }
            

            // Přesměruje zpět na aktuální stránku
            return Redirect(Request.Headers["Referer"].ToString());
        }

        //pomoc k emulaci
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var role = HttpContext.Session.GetString("EmulatedRole")
                ?? HttpContext.Session.GetString("Role");
            ViewData["CurrentRole"] = role;
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
            HttpContext.Session.SetString("Role", user.Role); //aktuální role
            HttpContext.Session.SetString("OriginalRole", user.Role); //Původní role

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

            // Nastavení cesty k profilce
            var profilePicturePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profile_pictures", $"{username}.jpg");
            zamestnanec.ProfilePictureUrl = System.IO.File.Exists(profilePicturePath)
                ? $"/images/profile_pictures/{username}.jpg"
                : "/images/default_pfp.jpg";


            return View(zamestnanec);
        }

        [HttpGet]
        public IActionResult EmployeeProfile(int id)
        {

            var role = GetCurrentRole(); // Použijeme metodu z BaseControlleru
            if (role != "Admin")
            {
                TempData["ErrorMessage"] = "Nemáte dostatečná oprávnění k provedení této akce.";
                return RedirectToAction("SearchEmploy", "DB");
            }

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
           // var employees = _connectionString.SearchZamestnanci(searchQuery ?? string.Empty);
            var employees = _connectionString.SearchEmployeees(searchQuery ?? string.Empty);


            return View(employees);
        }

        //uprava vlastnich informací
        [HttpGet]
        public IActionResult EditProfile()
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

            var viewModel = new ZamestnanecOsobniViewModel
            {
                IdZamestnanec = zamestnanec.IdZamestnanec,
                Jmeno = zamestnanec.Jmeno,
                Prijmeni = zamestnanec.Prijmeni,
                Email = zamestnanec.Email,
                Telefon = zamestnanec.Telefon,
                Pohlavi = zamestnanec.Pohlavi,
                IdAdresa = zamestnanec.IdAdresa
            };

            ViewBag.AdresyList = _connectionString.GetAllAdresa()
        .Select(a => new SelectListItem
        {
            Value = a.IdAdresa.ToString(),
            Text = $"{a.Ulice}, {a.PSC}, {a.ObecNazev}"
        }).ToList();


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditProfile(ZamestnanecOsobniViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Předat znovu seznam adres při chybě validace
                ViewBag.AdresyList = _connectionString.GetAllAdresa()
                    .Select(a => new SelectListItem
                    {
                        Value = a.IdAdresa.ToString(),
                        Text = $"{a.Ulice}, {a.PSC}, {a.ObecNazev}"
                    }).ToList();

                return View(model);
            }
            try
            {
                //_connectionString.UpdateZamestnanec(zamestnanec);
                _connectionString.UpdateZamestnanecOsobniUdaje(model);
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Chyba při aktualizaci profilu:" + ex.Message);
                ModelState.AddModelError("", "Aktualizace se nezdařila. Zkuste to prosím znovu.");

                ViewBag.AdresyList = _connectionString.GetAllAdresa()
            .Select(a => new SelectListItem
            {
                Value = a.IdAdresa.ToString(),
                Text = $"{a.Ulice}, {a.PSC}, {a.ObecNazev}"
            }).ToList();

                return View(model);
            }
        }

        //uprava informací jako plat atd.. pro manažera

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            //získání aktualní role (emulovaný nebo skutečný)

            var role = GetCurrentRole();
            //Kontrola oprávnění - pouze admin může upravovat zaměstnance
            if ( role != "Admin")
            {
                return Forbid();
            }
            var zamestnanec = _connectionString.GetZamestnanecById(id);
            if (zamestnanec == null)
            {
                return NotFound();
            }

            var viewModel = new ZamestnanecPoziceViewModel
            {
                IdZamestnanec = zamestnanec.IdZamestnanec,
                Role = zamestnanec.Role,
                Pozice = zamestnanec.Pozice,
                Plat = zamestnanec.Plat,
                TypSmlouva = zamestnanec.TypSmlouva,
                IdOddeleni = zamestnanec.IdOddeleni
            };

            // Připravit seznam oddělení pro dropdown
            ViewBag.OddeleniList = _connectionString.GetAllOddeleni()
                .Select(o => new SelectListItem
                {
                    Value = o.IdOddeleni.ToString(),
                    Text = o.Nazev
                }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditEmployee(ZamestnanecPoziceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OddeleniList = _connectionString.GetAllOddeleni()
                    .Select(o => new SelectListItem
                    {
                        Value = o.IdOddeleni.ToString(),
                        Text = o.Nazev
                    }).ToList();

                return View(model);
            }

            try
            {
                _connectionString.UpdateEmployeeDetails(model);
                
                return RedirectToAction("SearchEmploy", "DB");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Chyba při aktualizaci zaměstnance: " + ex.Message);
                return View(model);
            }
        }


        //na profilovku
        [HttpGet]
        public IActionResult ChangeProfilePicture()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult ChangeProfilePicture(IFormFile profilePicture)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Nastavíme název souboru podle uživatelského jména
                var fileName = $"{username}.jpg";

                // Cesta pro uložení souboru
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profile_pictures", fileName);

                // Uložení souboru
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    profilePicture.CopyTo(stream);
                }
            }

            return RedirectToAction("Profile");
        }





    }
}
