namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class HierarchieZamestnancu
    {
        public int IdZamestnanec { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Pozice { get; set; }
        public int? NadrazenyId { get; set; }
    }
}
