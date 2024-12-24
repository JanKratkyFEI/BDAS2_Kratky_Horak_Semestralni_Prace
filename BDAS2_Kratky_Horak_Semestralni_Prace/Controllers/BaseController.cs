using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Získání originální role
            var originalRole = HttpContext.Session.GetString("Role");
            ViewData["OriginalRole"] = originalRole;

            // Získání emulované role
            var emulatedRole = HttpContext.Session.GetString("EmulatedRole");
            ViewData["EmulatedRole"] = emulatedRole ?? originalRole; // Pokud není emulace, použije originální roli
        }

        protected string GetCurrentRole()
        {
            // Vrátí buď emulovanou roli, nebo originální roli
            return HttpContext.Session.GetString("EmulatedRole")
                ?? HttpContext.Session.GetString("Role");
        }
    }

}
