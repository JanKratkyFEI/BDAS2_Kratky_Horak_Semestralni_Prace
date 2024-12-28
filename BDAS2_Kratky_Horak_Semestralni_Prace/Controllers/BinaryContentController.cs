using BDAS2_Kratky_Horak_Semestralni_Prace.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class BinaryContentController : BaseController
    {
        private readonly OracleDatabaseHelper _connectionString;

        public BinaryContentController(OracleDatabaseHelper databaseHelper)
        {
            _connectionString = databaseHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var files = _connectionString.GetAllFiles();
            return View(files);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                // Výpis všech chyb do konzole
                foreach (var modelState in ModelState)
                {
                    if (modelState.Value.Errors.Count > 0)
                    {
                        foreach (var error in modelState.Value.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Chyba: {modelState.Key} - {error.ErrorMessage}");
                        }
                    }
                }

                return View();
            }
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Musíte vybrat soubor.");
                return View();
            }

            try
            {
                var employeeId = SessionHelper.GetCurrentEmployeeId(HttpContext, _connectionString);
                // Získání údajů o souboru
                var model = new BinarniObsah
                {
                    NazevSouboru = Path.GetFileNameWithoutExtension(file.FileName),
                    PriponaSouboru = Path.GetExtension(file.FileName),
                    TypSouboru = file.ContentType,
                    DatumNahrani = DateTime.Now,
                    Obsah = GetFileBytes(file), // Přečtení souboru do byte[]
                    Operace = "Nahrání",
                    IdZamestnanec = employeeId // Implementuj metodu pro získání aktuálního zaměstnance
                };

                // Uložení do databáze
                _connectionString.AddFile(model);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Chyba při nahrávání souboru: {ex.Message}");
                return View();
            }
        }

        //přečtení souboru do byte[]

        private byte[] GetFileBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var file = _connectionString.GetFileById(id);
            if (file == null)
            {
                return NotFound();
            }
            return View(file);
        }

        [HttpPost]
        public IActionResult Edit(BinarniObsah model)
        {
            if (ModelState.IsValid)
            {
                _connectionString.UpdateFile(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var file = _connectionString.GetFileById(id);
            if (file == null)
            {
                return NotFound();
            }
            return View(file);
        }


        [HttpPost]
        public IActionResult Delete(BinarniObsah model)
        {
            _connectionString.DeleteFile(model.IdObsah);
            return RedirectToAction("Index");
        }
    
    }
}
