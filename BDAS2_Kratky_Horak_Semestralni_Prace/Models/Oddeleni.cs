namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Oddeleni
    {
        //PK
        public int IdOddeleni { get; set; }
        //Attributy
        public string Nazev {  get; set; }
        public string Popis {  get; set; }

        //Navigační vlastnosti
        public ICollection<Zamestanec> Zamestnanci { get; set; }
    }
}
