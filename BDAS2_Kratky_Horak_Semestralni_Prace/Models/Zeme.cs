using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Zeme
    {

        //PK
        public int IdZeme { get; set; }
        //Atributy
        public string Nazev { get; set; }
        public int StupenNebezpeci { get; set; }

        //Kolekce obcí (1:N)
        [ValidateNever]
        public ICollection<Obec> Obce { get; set; }

    }
}
