using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Oddeleni
    {
        //PK
        public int IdOddeleni { get; set; }
        //Attributy
        public string Nazev {  get; set; }
        public int IdMuzeum { get; set; }
        public string MuzeumNazev { get; set; }

        //Navigační vlastnosti
        [ValidateNever]
        public ICollection<Zamestnanec> Zamestnanci { get; set; }
    }
}
