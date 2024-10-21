﻿namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
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

        //navigační vlastnosti pro relace
        public Sbirka Sbirka { get; set; } 
        public StavPredmetu StavPredmetu { get; set; }

        //Relace
        public ICollection<AutorPredmet> AutorPredmety { get; set; }
        public ICollection<PredmetMaterial> PredmetMaterialy { get; set; }

    }
}