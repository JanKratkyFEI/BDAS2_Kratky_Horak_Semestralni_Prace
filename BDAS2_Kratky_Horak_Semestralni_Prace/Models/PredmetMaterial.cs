namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class PredmetMaterial
    {
        public int IdMaterial { get; set; }
        public int IdPredmet {  get; set; }

        //Navigační vlastnosti pro propojení s entitami Material a Predmet
        public Material Material { get; set; }
        public Predmet Predmet { get; set; }
    }
}
