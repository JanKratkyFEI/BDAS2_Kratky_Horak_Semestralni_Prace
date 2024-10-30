namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class Material
    {
        //PK
        public int IdMaterial { get; set; }
        //Atributy
        public string Nazev { get; set; }

        //Navigační vlastnosti
        public ICollection<PredmetMaterial> PredmetyMaterialy { get; set; }
    }
}
