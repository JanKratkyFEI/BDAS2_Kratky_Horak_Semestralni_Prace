using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class StavPredmetu
    {
        //PK
        public int IdStav {  get; set; }

        //Atributy

        [Required(ErrorMessage = "Stav je povinný.")]
        public string Stav { get; set; }

        [Required(ErrorMessage = "Začátek stavu je povinný.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ZacatekStav { get; set; }

        [Required(ErrorMessage = "Konec stavu je povinný.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]

        public DateTime KonecStav { get; set; }

        //navigační vlastnost pro spojeni s Predmety
        [ValidateNever]
        public ICollection<Predmet> Predmety { get; set; }

    }
}