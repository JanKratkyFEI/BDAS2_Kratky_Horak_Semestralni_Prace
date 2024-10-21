namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Zamestnanec
    {
        //PK
        public int IdZamestnanec { get; set; }
        //atributy
        public string Jmeno {  get; set; }
        public string Prijmeni { get; set; }
        public string Pozice { get; set; }
        public DateTime DatumNastupu { get; set; }
        //FK
        public int IdOddeleni { get; set; } //Odkaz na oddělení , ve kterém zaměstnanec pracuje
        public int IdAdresa { get; set; } //odkaz na adresu zamestnance
        //Navigační vlastnosti test

        public Oddeleni Oddeleni { get; set; }
        public Adresa Adresa { get; set; }
    }
}
