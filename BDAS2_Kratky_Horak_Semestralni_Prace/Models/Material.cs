using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Material
    {
        //PK
        public int IdMaterial { get; set; }
        //Atributy
        public string Nazev { get; set; }

        //Navigační vlastnosti
        [ValidateNever]
        public ICollection<PredmetMaterial> PredmetyMaterialy { get; set; }
    }
}
