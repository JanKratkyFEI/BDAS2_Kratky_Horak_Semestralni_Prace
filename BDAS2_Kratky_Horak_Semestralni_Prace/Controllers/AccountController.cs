using BDAS2_Kratky_Horak_Semestralni_Prace.Helpers;
using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
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

        //registrace
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Register(RegisterViewModel model)
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

        //obsahuje TRANSAKCI
        [HttpPost]
        public IActionResult RegisterNewEmployee(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            //očištění
            model.Username = model.Username.Trim();
            model.Email = model.Email.Trim();
            model.Jmeno = model.Jmeno.Trim();
            model.Prijmeni = model.Prijmeni.Trim();

            //validace emailu
            var emailIsValid = _connectionString.IsEmailValid(model.Email);
            if (!emailIsValid)
            {
                ModelState.AddModelError("Email", "Zadaný e-mail není platný.");
                return View("Register", model);
            }


            var novyZamestnanec = new Zamestnanec
            {
                Jmeno = model.Jmeno,
                Prijmeni = model.Prijmeni,
                Username = model.Username,
                Password = PasswordHelper.HashPassword(model.Password), // Heslo bude hashované
                Email = model.Email,
                Role = "registered", // Výchozí role
                Plat = 18900, // Výchozí plat (nastavit podle aktuální minimální mzdy)
                DatumZamestnani = DateTime.Now, // Datum registrace jako datum zaměstnání
                IdRecZamestnanec = 1, //test test
                Pozice = "Nový kolega", // Výchozí hodnota pro pozici
                Telefon = "N/A", // Defaultní hodnota, pokud nemáme ve formuláři
                RodCislo = "N/A", // Defaultní hodnota, pokud není uvedeno
                TypSmlouva = "Plný úvazek", // Výchozí hodnota
                IdAdresa = 1, // Výchozí hodnota
                IdOddeleni = 1 // Výchozí hodnota
            };

            using (var connection = new OracleConnection(_connectionString.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Vložení zaměstnance
                        _connectionString.InsertZamestnanec(novyZamestnanec, connection, transaction);

                        // Potvrzení transakce
                        transaction.Commit();
                        return RedirectToAction("Login");
                    }
                    catch (Exception ex)
                    {
                        // Vrácení změn v případě chyby
                        transaction.Rollback();
                        
                        ModelState.AddModelError("", "Registrace se nezdařila. Zkuste to prosím znovu.");
                        return View("Register", model);
                    }
                }
            

            //try
            //{
            //    _connectionString.InsertZamestnanec(novyZamestnanec);
            //    return RedirectToAction("Login");
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("Chyba při registraci: " + ex.Message);
            //    System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

            //    ModelState.AddModelError("", "Registrace se nezdařila. Zkuste to prosím znovu.");
            //    return View("Register", model);
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

        //přihlašování
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

            if (user == null || string.IsNullOrEmpty(user.Password))
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



      


        //odhlášení
        [HttpPost]
        public IActionResult Logout()
        {
            //vyčistí session při odhlášení
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        //profil
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

            
            // Získání ID profilového obrázku z databáze
            var profilePictureId = _connectionString.GetProfilePictureId(zamestnanec.IdZamestnanec);
            zamestnanec.ProfilePictureUrl = GetProfilePicture(profilePictureId);



            return View(zamestnanec);
        }
        // profil jiného zaměstnance
        [HttpGet]
        public IActionResult EmployeeProfile(int id)
        {

            //var role = GetCurrentRole(); // Použijeme metodu z BaseControlleru
            //if (role != "Admin")
            //{
            //    TempData["ErrorMessage"] = "Nemáte dostatečná oprávnění k provedení této akce.";
            //    return RedirectToAction("SearchEmploy", "DB");
            //}

            var zamestnanec = _connectionString.GetZamestnanecFromView(id);
            if (zamestnanec == null)
            {
                return NotFound();
            }

            return View(zamestnanec);
        }
        // vyhledávání jiných zaměstnanců
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
                var emailIsValid = _connectionString.IsEmailValid(model.Email);
                if (!emailIsValid)
                {
                    ModelState.AddModelError("Email", "Zadaný e-mail není platný.");
                    ViewBag.AdresyList = _connectionString.GetAllAdresa()
                        .Select(a => new SelectListItem
                        {
                            Value = a.IdAdresa.ToString(),
                            Text = $"{a.Ulice}, {a.PSC}, {a.ObecNazev}"
                        }).ToList();

                    return View(model);
                }
                //_connectionString.UpdateZamestnanec(zamestnanec);
                _connectionString.UpdateZamestnanecOsobniUdaje(model);
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Aktualizace se nezdařila. Zkuste to prosím znovu.");

             

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
            if (role != "Admin")
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
                IdOddeleni = zamestnanec.IdOddeleni,
                IdRecZamestnanec = zamestnanec.IdRecZamestnanec
            };

            // Připravit seznam oddělení pro dropdown
            ViewBag.OddeleniList = _connectionString.GetAllOddeleni()
                .Select(o => new SelectListItem
                {
                    Value = o.IdOddeleni.ToString(),
                    Text = o.Nazev
                }).ToList();

            ViewBag.EmployeesList = _connectionString.GetAllZamestnanci()
                .Where(e => e.IdZamestnanec != id) // Zamezíme výběru sebe sama
                .Select(e => new SelectListItem
                {
                    Value = e.IdZamestnanec.ToString(),
                    Text = $"{e.Jmeno} {e.Prijmeni} ({e.Pozice})"
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

                ViewBag.EmployeesList = _connectionString.GetAllZamestnanci()
                    .Where(e => e.IdZamestnanec != model.IdZamestnanec)
                    .Select(e => new SelectListItem
                    {
                        Value = e.IdZamestnanec.ToString(),
                        Text = $"{e.Jmeno} {e.Prijmeni} ({e.Pozice})"
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

        //hiearchie
        [HttpGet]
        public IActionResult Hierarchie(int maxLevel = 3)
        {
            try
            {
                var hierarchie = _connectionString.GetHierarchieZamestnancu(maxLevel);
                return View(hierarchie);
            }
            catch (Exception ex)
            {
                // Záznam chyby do logu
                System.Diagnostics.Debug.WriteLine($"Chyba při získávání hierarchie: {ex.Message}");
                return StatusCode(500, "Došlo k chybě při získávání hierarchie.");
            }
        }


        //na profilovku
        [HttpGet]
        public IActionResult ChangeProfilePicture()
        {
            try
            {
                var currentEmployeeId = SessionHelper.GetCurrentEmployeeId(HttpContext, _connectionString);
                var files = _connectionString.GetFilesByEmployeeId(currentEmployeeId);

                return View(files);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Chyba: {ex.Message}");
                return RedirectToAction("Profile");
            }
        }

        [HttpPost]
        public IActionResult ChangeProfilePicture(int selectedFileId)
        {
            try
            {
                var currentEmployeeId = SessionHelper.GetCurrentEmployeeId(HttpContext, _connectionString);

                // Aktualizace odkazu na profilový obrázek
                _connectionString.UpdateProfilePicture(currentEmployeeId, selectedFileId);

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Chyba: {ex.Message}");
                return RedirectToAction("Profile");
            }
        }


        private string GetProfilePicture(int? profilePicId)
            {
            System.Diagnostics.Debug.WriteLine($"Generating picture URL for ProfilePictureId={profilePicId}");

            if (!profilePicId.HasValue)
            {
                return "/images/default_pfp.jpg"; // Výchozí obrázek
            }

            var url = Url.Action("GetFile", "BinaryContent", new { id = profilePicId.Value });
            System.Diagnostics.Debug.WriteLine($"Generated URL: {url}");
            return url;
        }

        //metoda pro systémový katalog
        [HttpGet]
        public IActionResult DatabaseObjects()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return Forbid();
            }

            var dbObjects = _connectionString.GetDatabaseObjects();
            return View(dbObjects);
        }

        //metoda pro loggování

        [HttpGet]
        public IActionResult History()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return Forbid();
            }

            var logs = _connectionString.GetHistoryLogs();
            return View(logs);
        }


        [HttpGet]
        public IActionResult ViewLogs()
        {
            try
            {
                var logs = _connectionString.GetHistoryLogs();
                return View(logs);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Chyba při načítání logů: {ex.Message}");
                return View(new List<HistoryLog>()); // V případě chyby vrací prázdný seznam
            }
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _connectionString.GetZamestnanecById(id); // Získání zaměstnance podle ID
            if (employee == null)
            {
                return NotFound("Zaměstnanec nebyl nalezen.");
            }

            return View(employee); // Zobrazí potvrzovací stránku s detailem zaměstnance
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var currentUserRole = HttpContext.Session.GetString("Role");
                if (currentUserRole != "Admin")
                {
                    return Forbid(); // Pouze admin může mazat zaměstnance
                }

                _connectionString.DeleteZamestnanec(id); // Smazání zaměstnance v databázi
                return RedirectToAction("Index"); // Přesměrování zpět na seznam zaměstnanců
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Došlo k chybě při mazání: {ex.Message}");
                return RedirectToAction("Delete", new { id });
            }
        }

        // Pomocná metoda pro získání ID aktuálního uživatele
        private int GetCurrentUserId()
        {
            return int.Parse(HttpContext.Session.GetString("UserId"));
        }







    }
}

