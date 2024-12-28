using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Obec
    {
        //PK
        public int IdObec { get; set; }

        //Atributy
        public string Nazev { get; set; }
        public int IdZeme { get; set; } //Každá obec patří jedné zemi

        [ValidateNever]
        public string ZemeNazev { get; set; } //ID_ZEME translated to string

        //Navigační vlastnosti
        [ValidateNever]
        public Zeme zeme { get; set; } //navigančí vlasntost pro relaci s entitou zeme

        // Kolekce adres (1:N)
        [ValidateNever]
        public ICollection<Adresa> Adresy { get; set; } //Navigační vlastnost pro relaci s adresami
    }
}
