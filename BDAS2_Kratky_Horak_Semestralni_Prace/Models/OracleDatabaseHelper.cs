using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{

    public class OracleDatabaseHelper
    {
        private readonly string _connectionString;

        public OracleDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Autor> GetAutori()
        {
            var autori = new List<Autor>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                // Dotaz na tabulku AUTOR
                var query = "SELECT ID_AUTOR, JMENO, PRIJMENI FROM AUTOR";
                {
                    using (var command = new OracleCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                autori.Add(new Autor
                                {
                                    IdAutor = reader.GetInt32(reader.GetOrdinal("IdAutor")),
                                    Jmeno = reader.GetString(reader.GetOrdinal("Jmeno")),
                                    Prijmeni = reader.GetString(reader.GetOrdinal("Prijmeni"))
                                });
                            }
                        }
                    }
                } 
            }
            return autori;
        }

            public List<Predmet> GetPredmety(string typ = null)
        {
            var predmety = new List<Predmet>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                //Sestavení dotazu na základě parametru typ
                var query = "SELECT Id, Nazev, Popis, Typ FROM Predmet";
                if (!string.IsNullOrEmpty(typ))
                {
                    query += "WHERE Typ = :typ";
                }

                using (var command = new OracleCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(typ))
                    {
                        command.Parameters.Add(new OracleParameter("typ", typ));
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var predmet = new Predmet
                            {
                                IdPredmet = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nazev = reader.GetString(reader.GetOrdinal("Nazev")),
                                Popis = reader.GetString(reader.GetOrdinal("Popis")),
                                Typ = reader.GetString(reader.GetOrdinal("Typ"))
                            };

                            //Rozlišení podle typu a naplnění specifických atributů
                            switch (predmet.Typ)
                            {
                                case "Fotografie":
                                    predmet = new Fotografie
                                    {
                                        IdPredmet = predmet.IdPredmet,
                                        Nazev = predmet.Popis,
                                        Typ = predmet.Typ,
                                        Zanr = reader.GetString(reader.GetOrdinal("Zanr")),
                                        License = reader.GetString(reader.GetOrdinal("License"))
                                    };
                                    break;

                                case "Obraz":
                                    predmet = new Obraz
                                    {
                                        IdPredmet = predmet.IdPredmet,
                                        Nazev = predmet.Nazev,
                                        Popis = predmet.Popis,
                                        Typ = predmet.Typ,
                                        UmeleckyStyl = reader.GetString(reader.GetOrdinal("UmeleckyStyl")),
                                        Medium = reader.GetString(reader.GetOrdinal("Medium"))
                                    };
                                    break;
                                case "Socha":
                                    predmet = new Socha
                                    {
                                        IdPredmet = predmet.IdPredmet,
                                        Nazev = predmet.Nazev,
                                        Popis = predmet.Popis,
                                        Typ = predmet.Typ,
                                        Vaha = (double)reader.GetDecimal(reader.GetOrdinal("Vaha")),
                                        TechnikaTvorby = reader.GetString(reader.GetOrdinal("TechnikaTvorby"))
                                    };
                                    break;
                            }
                            predmety.Add(predmet);
                        }
                    }
                }
            }
            return predmety;
        }

        public List<Zamestnanec> GetZamestnanci()
        {
            var zamestnanci = new List<Zamestnanec>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
            SELECT 
                IdZamestnanec, Pozice, Jmeno, Prijmeni, Email, Telefon, 
                RodneCislo, DatumZamestnani, TypSmlouva, Plat, Pohlavi, 
                IdAdresa, IdOddeleni, IdRecZamestnanec
            FROM Zamestnanec";

                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zamestnanci.Add(new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("IdZamestnanec")),
                                Pozice = reader.GetString(reader.GetOrdinal("Pozice")),
                                Jmeno = reader.GetString(reader.GetOrdinal("Jmeno")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("Prijmeni")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Tel = reader.GetString(reader.GetOrdinal("Telefon")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RodneCislo")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DatumZamestnani")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TypSmlouva")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("Plat")),
                                Pohlavi = reader.GetInt32(reader.GetOrdinal("Pohlavi")),
                                IdAdresa = reader.GetInt32(reader.GetOrdinal("IdAdresa")),
                                IdOddeleni = reader.GetInt32(reader.GetOrdinal("IdOddeleni")),
                                IdRecZamestnanec = reader.GetInt32(reader.GetOrdinal("IdRecZamestnanec"))
                            });
                        }
                    }
                }
            }

            return zamestnanci;
        }

        public List<Adresa> GetAdresy()
        {
            var adresy = new List<Adresa>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT IdAdresa, Ulice, PSC, IdObec, CP, IdMuzeum FROM Adresa";

                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            adresy.Add(new Adresa
                            {
                                IdAdresa = reader.GetInt32(reader.GetOrdinal("IdAdresa")),
                                Ulice = reader.GetString(reader.GetOrdinal("Ulice")),
                                PSC = reader.GetString(reader.GetOrdinal("PSC")),
                                IdObec = reader.GetInt32(reader.GetOrdinal("IdObec")),
                                CP = reader.GetString(reader.GetOrdinal("CP")),
                                IdMuzeum = reader.GetInt32(reader.GetOrdinal("IdMuzeum"))
                            });
                        }
                    }
                }
            }

            return adresy;
        }




        public void InsertPredmet(Predmet predmet)
                {
                    using (OracleConnection conn = new OracleConnection(_connectionString))
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

        public Autor GetAutorById(int id)
        {
            Autor autor = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT IdAutor, Jmeno, Prijmeni FROM Autor WHERE IdAutor = :id";
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            autor = new Autor
                            {
                                IdAutor = reader.GetInt32(reader.GetOrdinal("IdAutor")),
                                Jmeno = reader.GetString(reader.GetOrdinal("Jmeno")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("Prijmeni"))
                            };
                        }
                    }
                }
            }

            return autor;
        }
        public Adresa GetAdresaById(int id)
        {
            Adresa adresa = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT IdAdresa, Ulice, PSC, IdObec, CP, IdMuzeum FROM Adresa WHERE IdAdresa = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            adresa = new Adresa
                            {
                                IdAdresa = reader.GetInt32(reader.GetOrdinal("IdAdresa")),
                                Ulice = reader.GetString(reader.GetOrdinal("Ulice")),
                                PSC = reader.GetString(reader.GetOrdinal("PSC")),
                                IdObec = reader.GetInt32(reader.GetOrdinal("IdObec")),
                                CP = reader.GetString(reader.GetOrdinal("CP")),
                                IdMuzeum = reader.GetInt32(reader.GetOrdinal("IdMuzeum"))
                            };
                        }
                    }
                }
            }

            return adresa;
        }



        public void TestConnection()
                {
                    try
                    {
                        using (OracleConnection conn = new OracleConnection(_connectionString))
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
