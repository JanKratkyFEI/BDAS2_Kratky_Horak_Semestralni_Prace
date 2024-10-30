namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Adresa
    {

        //PK
        public int IdAdresa { get; set; }

        //Atributy
        public string Ulice { get; set; }
        public string Psc { get; set; }
        public string Cp { get; set; }

        //FK
        public int IdObec { get; set; }
        public int IdMuzeum { get; set; }

        //Navigační vlastnosti
        public Obec Obec { get; set; } //Navigační vlastnost pro obec
        public Muzeum Muzeum { get; set; } //Navigační vlasntost pro muzeum

        //Relace
        public ICollection<Zamestnanec> Zamestnanci { get; set; } //Adresa může mít přiřazené zaměstnance
    }
}

