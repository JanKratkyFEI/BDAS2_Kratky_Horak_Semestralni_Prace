namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Muzeum
    {
//PK
public int  IdMuzeum { get; set; }

//Artributy
public string Nazev { get; set; }

//Kolekce sbírek (1:N)

        public ICollection<Sbirka> Sbirky { get; set; } //Navigační vlastnost pro relaci se sbírkami

        //KOlekce adres (1:N)
        public ICollection<Adresa> Adresy { get; set; }





    }
}
