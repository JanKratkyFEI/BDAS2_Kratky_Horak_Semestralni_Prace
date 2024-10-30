namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Obec
    {
        //PK
        public int IdObec { get; set; }

        //Atributy
        public string Nazev { get; set; }
        public int IdZeme { get; set; } //Každá obec patří jedné zemi

        //Navigační vlastnosti
        public Zeme zeme { get; set; } //navigančí vlasntost pro relaci s entitou zeme

        // Kolekce adres (1:N)
        public ICollection<Adresa> Adresy { get; set; } //Navigační vlastnost pro relaci s adresami
    }
}
