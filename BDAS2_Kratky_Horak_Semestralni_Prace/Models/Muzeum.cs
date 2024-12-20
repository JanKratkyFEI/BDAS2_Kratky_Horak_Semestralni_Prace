using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Muzeum
    {
//PK
public int  IdMuzeum { get; set; }

//Artributy
public string Nazev { get; set; }

        //Kolekce sbírek (1:N)
        [ValidateNever]
        public ICollection<Sbirka> Sbirky { get; set; } //Navigační vlastnost pro relaci se sbírkami
        [ValidateNever]
        //KOlekce adres (1:N)
        public ICollection<Adresa> Adresy { get; set; }





    }
}
