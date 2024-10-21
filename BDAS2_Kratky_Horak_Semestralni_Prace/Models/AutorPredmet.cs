namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class AutorPredmet
    {
        
        public int IdAutor { get; set; }
        
        public Autor Autor { get; set; }
        public int IdPredmet { get; set; }
        public Predmet Predmet { get; set; }
    }
}
