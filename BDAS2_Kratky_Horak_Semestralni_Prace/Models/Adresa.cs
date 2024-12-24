using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Adresa
    {

        //PK
        public int IdAdresa { get; set; }

        //Atributy
        public string Ulice { get; set; }
        public string PSC { get; set; }
        //FK
        public int IdObec { get; set; }
        public string CP { get; set; }
        //FK
        public int IdMuzeum { get; set; }

        // Navigační vlastnosti
        [ValidateNever]
        public string ObecNazev { get; set; } // Název obce
        [ValidateNever]
        public string MuzeumNazev { get; set; } // Název muzea


        //Navigační vlastnosti
        [ValidateNever]
        public Obec Obec { get; set; } //Navigační vlastnost pro obec
        [ValidateNever]
        public Muzeum Muzeum { get; set; } //Navigační vlasntost pro muzeum

        //Relace
        [ValidateNever]
        public ICollection<Zamestnanec> Zamestnanci { get; set; } //Adresa může mít přiřazené zaměstnance
    }
}

