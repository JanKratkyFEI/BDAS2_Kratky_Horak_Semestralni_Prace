using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Autor
    {
        //PK
        public int IdAutor { get; set; }

        //Atributy
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }

        //Navigační vlastnosti
        [ValidateNever]
        public ICollection<AutorPredmet> AutorPredmety { get; set; } //Relace mnoho-na-mnoho s předměty
    }
}
