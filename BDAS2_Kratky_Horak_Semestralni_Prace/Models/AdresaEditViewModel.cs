using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class AdresaEditViewModel
    {
        public Adresa Adresa { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Obce { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Muzea { get; set; }
    }


}
