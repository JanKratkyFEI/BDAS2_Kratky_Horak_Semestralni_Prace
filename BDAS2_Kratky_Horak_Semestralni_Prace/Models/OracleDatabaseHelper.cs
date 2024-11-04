using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
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

        //ADD METODY
        public void AddZamestnanec(Zamestnanec zamestnanec)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
            INSERT INTO Zamestnanec (ID_ZAMESTNANEC, JMENO, PRIJMENI, USERNAME, PASSWORD, EMAIL)
            VALUES (S_ZAMESTNANEC.nextval, :jmeno, :prijmeni, :username, :password, :email)";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("jmeno", zamestnanec.Jmeno));
                    command.Parameters.Add(new OracleParameter("prijmeni", zamestnanec.Prijmeni));
                    command.Parameters.Add(new OracleParameter("username", zamestnanec.Username));
                    command.Parameters.Add(new OracleParameter("password", zamestnanec.Password));
                    command.Parameters.Add(new OracleParameter("email", zamestnanec.Email));

                    command.ExecuteNonQuery();
                }
            }
        }
        public void AddMaterial(Material material)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO MATERIAL (ID_MATERIAL, NAZEV) VALUES (S_MATERIAL.nextval, :nazev)";

                using(var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("nazev", material.Nazev));
                    command.ExecuteNonQuery();
                }
            }
        }
        public void AddZeme(Zeme zeme)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO ZEME (ID_ZEME, NAZEV, STUPEN_NEBEZPECI) VALUES (S_ZEME.nextval, :nazev, :stupenNebezpeci)";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("nazev", zeme.Nazev));
                    command.Parameters.Add(new OracleParameter("stupenNebezpeci", zeme.StupenNebezpeci));
                    command.ExecuteNonQuery();
                }
            }
        }




        //GET METODY
        public List<Zeme> GetAllZeme()
        {
            var zemeList = new List<Zeme>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT ID_ZEME, NAZEV, STUPEN_NEBEZPECI FROM ZEME";

                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zemeList.Add(new Zeme
                            {
                                IdZeme = reader.GetInt32(reader.GetOrdinal("ID_ZEME")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                                StupenNebezpeci = reader.GetInt32(reader.GetOrdinal("STUPEN_NEBEZPECI"))
                            });
                        }
                    }
                }
            }

            return zemeList;
        }
        public Zeme GetZemeById(int id)
        {
            Zeme zeme = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT ID_ZEME, NAZEV, STUPEN_NEBEZPECI FROM ZEME WHERE ID_ZEME = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            zeme = new Zeme
                            {
                                IdZeme = reader.GetInt32(reader.GetOrdinal("ID_ZEME")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                                StupenNebezpeci = reader.GetInt32(reader.GetOrdinal("STUPEN_NEBEZPECI"))
                            };
                        }
                    }
                }
            }

            return zeme;
        }

        public List<Material> GetAllMaterials()
        {
            var materials = new List<Material>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT ID_MATERIAL, NAZEV FROM MATERIAL";
                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            materials.Add(new Material
                            {
                                IdMaterial = reader.GetInt32(reader.GetOrdinal("ID_MATERIAL")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV"))
                            });
                        }
                    }
                }
            }

            return materials;

        }

        public Material GetMaterialById(int id) 
        {
            Material material = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT ID_MATERIAL, NAZEV FROM MATERIAL WHERE ID_MATERIAL = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            material = new Material
                            {
                                IdMaterial = reader.GetInt32(reader.GetOrdinal("ID_MATERIAL")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV"))
                            };
                        }
                    }
                }
            }

            return material;
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
                                        Licence = reader.GetString(reader.GetOrdinal("License"))
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
        IdAdresa, IdOddeleni, IdRecZamestnanec, Username, Password
        FROM Zamestnanec
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
                                Telefon = reader.GetString(reader.GetOrdinal("Telefon")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RodneCislo")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DatumZamestnani")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TypSmlouva")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("Plat")),
                                Pohlavi = reader.GetInt32(reader.GetOrdinal("Pohlavi")),
                                IdAdresa = reader.GetInt32(reader.GetOrdinal("IdAdresa")),
                                IdOddeleni = reader.GetInt32(reader.GetOrdinal("IdOddeleni")),
                                IdRecZamestnanec = reader.GetInt32(reader.GetOrdinal("IdRecZamestnanec")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Password"))
                            
                            });
                        }
                    }
                }
            }

            return zamestnanci;
        }

        public Zamestnanec GetZamestnanecByUsername(string username)
        {
            Zamestnanec zamestnanec = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
            SELECT
            IdZamestnanec, Pozice, Jmeno, Prijmeni, Email, Telefon, 
            RodneCislo, DatumZamestnani, TypSmlouva, Plat, Pohlavi, 
            IdAdresa, IdOddeleni, IdRecZamestnanec, Username, Password
            FROM Zamestnanec
            WHERE Username = :username";
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("username", username));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            zamestnanec = new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("IdZamestnanec")),
                                Pozice = reader.GetString(reader.GetOrdinal("Pozice")),
                                Jmeno = reader.GetString(reader.GetOrdinal("Jmeno")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("Prijmeni")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Telefon = reader.GetString(reader.GetOrdinal("Telefon")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RodneCislo")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DatumZamestnani")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TypSmlouva")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("Plat")),
                                Pohlavi = reader.GetInt32(reader.GetOrdinal("Pohlavi")),
                                IdAdresa = reader.GetInt32(reader.GetOrdinal("IdAdresa")),
                                IdOddeleni = reader.GetInt32(reader.GetOrdinal("IdOddeleni")),
                                IdRecZamestnanec = reader.GetInt32(reader.GetOrdinal("IdRecZamestnanec")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Password"))
                            };
                        }
                    }
                }
            }
            return zamestnanec;
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


        public string GetPredmetTypeById(int id)
        {
            string typ = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT TYP FROM PREDMET WHERE ID_PREDMET = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            typ = reader.GetString(reader.GetOrdinal("TYP"));
                        }
                    }
                }
            }
            return typ;
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

        public Fotografie GetFotografieById(int id)
        {
            Fotografie fotografie = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                SELECT p.ID_PREDMET, p.NAZEV, p.STARI, p.POPIS, p.TYP, p.ID_STAV, p.ID_SBIRKA,
                       f.ZANR, f.LICENCE
                FROM PREDMET p
                JOIN FOTOGRAFIE f ON p.ID_PREDMET = f.ID_PREDMET
                WHERE p.ID_PREDMET = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fotografie = new Fotografie
                            {
                                IdPredmet = reader.GetInt32(reader.GetOrdinal("ID_PREDMET")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                                Stari = reader.GetInt32(reader.GetOrdinal("STARI")),
                                Popis = reader.GetString(reader.GetOrdinal("POPIS")),
                                Typ = reader.GetString(reader.GetOrdinal("TYP")),
                                IdStav = reader.GetInt32(reader.GetOrdinal("ID_STAV")),
                                IdSbirka = reader.GetInt32(reader.GetOrdinal("ID_SBIRKA")),
                                Zanr = reader.GetString(reader.GetOrdinal("ZANR")),
                                Licence = reader.GetString(reader.GetOrdinal("LICENCE"))
                            };
                        }
                    }
                }
            }
            return fotografie;
        }

        public Obraz GetObrazById(int id)
        {
            Obraz obraz = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @" 
                    SSELECT p.ID_PREDMET, p.NAZEV, p.STARI, p.POPIS, p.TYP, p.ID_STAV, p.ID_SBIRKA,
                   o.UMELECKY_STYL, o.MEDIUM
                FROM PREDMET p
                JOIN OBRAZ o ON p.ID_PREDMET = o.ID_PREDMET
                WHERE p.ID_PREDMET = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));

                    using(var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            obraz = new Obraz
                            {
                                IdPredmet = reader.GetInt32(reader.GetOrdinal("ID_PREDMET")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                                Stari = reader.GetInt32(reader.GetOrdinal("STARI")),
                                Popis = reader.GetString(reader.GetOrdinal("POPIS")),
                                Typ = reader.GetString(reader.GetOrdinal("TYP")),
                                IdStav = reader.GetInt32(reader.GetOrdinal("ID_STAV")),
                                IdSbirka = reader.GetInt32(reader.GetOrdinal("ID_SBIRKA")),
                                UmeleckyStyl = reader.GetString(reader.GetOrdinal("UMELECKY_STYL")),
                                Medium = reader.GetString(reader.GetOrdinal("MEDIUM"))
                            };
                        }
                    }
                }
            }
            return obraz;
        }

        public Socha GetSochaById(int id)
        {
            Socha socha = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT p.ID_PREDMET, p.NAZEV, p.POPIS,p.TYP, p.ID_STAV,p.ID_SBIRKA,
                           s.VAHA, s.TECHNIKA_TVORBY
                    FROM PREDMET p
                    JOIN SOCHA s ON p.ID_PREDMET = s.ID_PREDMET
                    WHERE p.ID_PREDMET = :id";
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            socha = new Socha
                            {
                                IdPredmet = reader.GetInt32(reader.GetOrdinal("ID_PREDMET")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                                Popis = reader.GetString(reader.GetOrdinal("POPIS")),
                                Stari = reader.GetInt32(reader.GetOrdinal("STARI")),
                                Typ = reader.GetString(reader.GetOrdinal("TYP")),
                                IdStav = reader.GetInt32(reader.GetOrdinal("ID_STAV")),
                                IdSbirka = reader.GetInt32(reader.GetOrdinal("ID_SBIRKA")),
                                Vaha = (double)reader.GetDecimal(reader.GetOrdinal("VAHA")),
                                TechnikaTvorby = reader.GetString(reader.GetOrdinal("TECHNIKA_TVORBY"))
                            };
                        }
                    }
                }
            }
            return socha;
        }

        //UPDATE
        public void UpdateZeme(Zeme zeme)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE ZEME SET NAZEV = :nazev, STUPEN_NEBEZPECI = :stupenNebezpeci WHERE ID_ZEME = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("nazev", zeme.Nazev));
                    command.Parameters.Add(new OracleParameter("stupenNebezpeci", zeme.StupenNebezpeci));
                    command.Parameters.Add(new OracleParameter("id", zeme.IdZeme));
                    command.ExecuteNonQuery();
                }
            }
        }


        public void UpdateMaterial(Material material)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE MATERIAL SET NAZEV = :nazev WHERE ID_MATERIAL = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("nazev", material.Nazev));
                    command.Parameters.Add(new OracleParameter("id", material.IdMaterial));
                    command.ExecuteNonQuery();
                }
            }
        }

        //DELETE
        public void DeleteMaterial(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM MATERIAL WHERE ID_MATERIAL = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteZeme(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM ZEME WHERE ID_ZEME = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));
                    command.ExecuteNonQuery();
                }
            }
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
