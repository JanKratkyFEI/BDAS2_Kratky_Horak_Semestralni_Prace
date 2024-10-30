using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{

    public class OracleDatabaseHelper
    {
        private readonly string connectionString;

        public OracleDatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Autor> GetAutori()
        {
            var autori = new List<Autor>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                // Dotaz na tabulku AUTOR
                using (OracleCommand cmd = new OracleCommand("SELECT ID_AUTOR, JMENO, PRIJMENI FROM AUTOR", conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            autori.Add(new Autor
                            {
                                IdAutor = reader.GetInt32(0),
                                Jmeno = reader.GetString(1),
                                Prijmeni = reader.GetString(2),
                            });
                        }
                    }
                }
            }

            return autori;
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

        public void TestConnection()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open(); 
                    Console.WriteLine("Připojení k databázi bylo úspěšné.");

                    // Jednoduchý dotaz pro zobrazení dat z tabulky PREDMET
                    using (OracleCommand cmd = new OracleCommand("SELECT IdPredmet, Nazev FROM PREDMET", conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Data z tabulky PREDMET:");
                            while (reader.Read())
                            {
                                // Zobrazení dat - můžeme zobrazit Id a Název předmětu
                                Console.WriteLine($"ID: {reader["IdPredmet"]}, Název: {reader["Nazev"]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba při připojení k databázi " + ex.Message);
            }
        }
    }
}
