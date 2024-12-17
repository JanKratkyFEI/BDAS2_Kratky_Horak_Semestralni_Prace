using System.ComponentModel.DataAnnotations;
namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Jméno je povinné.")]
		public string Jmeno { get; set; }
        [Required(ErrorMessage = "Příjmení je povinné.")]
        public string Prijmeni { get; set; }

        [Required(ErrorMessage = "Email je povinné.")]
		[EmailAddress(ErrorMessage = "Neplatná emailová adresa.")]
		public string Email { get; set; }

        [Required(ErrorMessage = "Uživatelské jméno je povinné.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Heslo je povinné.")]
		[MinLength(3, ErrorMessage = "Heslo musí mít alespon 3 znaků.")]
        public string Password { get; set; }
		public bool IsExistingEmployee { get; set; } // Toto pole určí, zda uživatel zaškrtl, že je zaměstnanec
	}
}

