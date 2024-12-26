using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Predmet
    {
        //Primární klíč
        public int IdPredmet { get; set; }
        //atributy
        public string Nazev { get; set; }
        public int Stari { get; set; }
        public string Popis { get; set; }
        public string Typ { get; set; }

        //FK
        public int IdSbirka { get; set; } //Každý předmět patří do sbírky
        public int IdStav { get; set; } //Předmět má jeden stav


        //pro čitelnost ve view
        [ValidateNever]
        public string SbirkaNazev { get; set; }
        [ValidateNever]
        public string StavNazev { get; set; }

        //navigační vlastnosti pro relace
        [ValidateNever]
        public Sbirka Sbirka { get; set; }
        [ValidateNever]
        public StavPredmetu StavPredmetu { get; set; }

        //Relace
        [ValidateNever]
        public ICollection<AutorPredmet> AutorPredmety { get; set; }
        [ValidateNever]
        public ICollection<PredmetMaterial> PredmetMaterialy { get; set; }

    }
}
