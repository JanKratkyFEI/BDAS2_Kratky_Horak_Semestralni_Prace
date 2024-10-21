using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{

    public class OracleDatabaseHelper
    {
        private string connectionString = "our_connection_string_";

        public List<Predmet> GetPredmety()
        {
            var predmety = new List<Predmet>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT * FROM PREDMET", conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            predmety.Add(new Predmet
                            {
                                IdPredmet = reader.GetInt32(0),
                                Nazev = reader.GetString(1),
                                // další vlastnosti podle potřeby...
                            });
                        }
                    }
                }
            }

            return predmety;
        }

        public void InsertPredmet(Predmet predmet)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("INSERT INTO Predmet (Nazev, Stari,Popis) VALUES (:nazev, :stari, :popis)", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("nazev", predmet.Nazev));
                    cmd.Parameters.Add(new OracleParameter("stari", predmet.Stari));
                    cmd.Parameters.Add(new OracleParameter("popis", predmet.Popis));

                    cmd.ExecuteNonQuery(); // vykonání dotazu
                }
            }
        }
    }
}
