using System.Security.Cryptography;
using System.Text;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();

            }


        }

        public static bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            //Hash zadaného hesla
            string hashedInputPassword = HashPassword(enteredPassword);

            //porovná hash zadanýho hesla s uloženým hashovaným heslem
            return hashedInputPassword == hashedPassword;
        }
    }
}
