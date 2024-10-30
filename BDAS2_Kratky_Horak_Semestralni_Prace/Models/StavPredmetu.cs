namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class StavPredmetu
    {
        //PK
        public int IdStav {  get; set; }

        //Atributy
        public string Stav {  get; set; }
        public DateTime ZacatekStav { get; set; }
        public DateTime KonecStav { get; set; }

        //navigační vlastnost pro spojeni s Predmety
        public ICollection<Predmet> Predmety { get; set; }

    }
}