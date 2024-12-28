namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public static class SessionHelper
    {
        public static int GetCurrentEmployeeId(HttpContext httpContext, OracleDatabaseHelper connectionString)
        {
            var username = httpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidOperationException("Uživatel není přihlášen.");
            }

            // Získání zaměstnance podle username
            var zamestnanec = connectionString.GetZamestnanecByUsername(username);
            if (zamestnanec == null)
            {
                throw new InvalidOperationException("Zaměstnanec nebyl nalezen.");
            }

            return zamestnanec.IdZamestnanec;
        }
    }

}
