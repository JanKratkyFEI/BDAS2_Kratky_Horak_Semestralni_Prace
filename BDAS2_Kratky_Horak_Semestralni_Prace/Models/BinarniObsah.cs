namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class BinarniObsah
    {
        public int IdObsah { get; set; }
        public string NazevSouboru { get; set; }
        public string TypSouboru { get; set; }
        public string PriponaSouboru { get; set; }
        public byte[] Obsah { get; set; }
        public DateTime DatumNahrani { get; set; }
        public DateTime? DatumModifikace { get; set; }
        public string Operace { get; set; }
        public int IdZamestnanec { get; set; }

    }
}
