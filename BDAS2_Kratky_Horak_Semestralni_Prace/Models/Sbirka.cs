using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Sbirka
    {
        //PK
        public int IdSbirka { get; set; }

        //Atributy
        public string Nazev {  get; set; }
        public string Popis { get; set; }

        //FK
        public int IdMuzeum { get; set; } //Každá sbírka patří do muzea

        [ValidateNever]
        public string MuzeumNazev {  get; set; }
        //Navigační vlastnosti
        [ValidateNever]
        public Muzeum Muzeum { get; set; }

        //Kolekce předmětů v rámci sbírky (1:N)
        [ValidateNever]
        public ICollection<Predmet> Predmet { get; set; } // Navigační vlastnost pro relaci s entitou Predmet
    }
}
