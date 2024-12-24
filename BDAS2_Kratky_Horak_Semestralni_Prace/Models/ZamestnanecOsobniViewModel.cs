namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class ZamestnanecOsobniViewModel
    {
        public int IdZamestnanec { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public int? Pohlavi { get; set; }
        public int IdAdresa { get; set; }
    }

}
