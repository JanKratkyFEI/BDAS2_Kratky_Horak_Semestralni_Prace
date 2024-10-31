﻿namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Zamestnanec
    {
        //PK
        public int IdZamestnanec { get; set; }
        //atributy
        public string Pozice { get; set; }
        public string Jmeno {  get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string RodCislo { get; set; }
        public DateTime DatumZamestnani { get; set; }
        public string TypSmlouva { get; set; }
        public decimal Plat {  get; set; }
        public int Pohlavi { get; set; }
        //FK
        public int IdOddeleni { get; set; } //Odkaz na oddělení , ve kterém zaměstnanec pracuje
        public int IdAdresa { get; set; } //odkaz na adresu zamestnance

        public int IdRecZamestnanec { get; set; }
        //Navigační vlastnosti test

        public Oddeleni Oddeleni { get; set; }
        public Adresa Adresa { get; set; }

        public Zamestnanec RedZamestnanec { get; set; }
    }
}
