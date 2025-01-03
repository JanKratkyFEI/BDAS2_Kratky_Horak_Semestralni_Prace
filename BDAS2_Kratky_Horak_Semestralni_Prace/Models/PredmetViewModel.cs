﻿namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class PredmetViewModel
    {
        // Společné atributy pro všechny předměty
        public string Nazev { get; set; }

        public int? Stari { get; set; }
        public string? Popis { get; set; }
        public string Typ { get; set; } // Typ předmětu, např. "Fotografie", "Obraz", "Socha"

        // Specifické atributy pro Fotografie
        public string Zanr { get; set; }
        public string Licence { get; set; }

        public int id_stav { get; set; }

        public int id_sbirka {  get; set; }

        public string stavNazev { get; set; }
        public string sbirkaNazev { get; set; }

        // Specifické atributy pro Obraz
        public string UmeleckyStyl { get; set; }
        public string Medium { get; set; }

        // Specifické atributy pro Socha
        public double Vaha { get; set; }
        public string TechnikaTvorby { get; set; }
    }
}
