using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipelines;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    //třída slouží jako vrstva pro propojení a komunikaci s DB
    //CRUD metody
    public class OracleDatabaseHelper
    {
        private readonly string _connectionString;

        public OracleDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public string ConnectionString => _connectionString;

        //ADD METODY
        public void AddZamestnanec(Zamestnanec zamestnanec)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("INSERT_ZAMESTNANEC", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_Pozice", OracleDbType.Varchar2).Value = zamestnanec.Pozice;
                    command.Parameters.Add("p_Jmeno", OracleDbType.Varchar2).Value = zamestnanec.Jmeno;
                    command.Parameters.Add("p_Prijmeni", OracleDbType.Varchar2).Value = zamestnanec.Prijmeni;
                    command.Parameters.Add("p_Email", OracleDbType.Varchar2).Value = zamestnanec.Email;
                    command.Parameters.Add("p_Telefon", OracleDbType.Varchar2).Value = zamestnanec.Telefon;
                    command.Parameters.Add("p_RodneCislo", OracleDbType.Varchar2).Value = zamestnanec.RodCislo;
                    command.Parameters.Add("p_DatumZamestnani", OracleDbType.Date).Value = zamestnanec.DatumZamestnani;
                    command.Parameters.Add("p_TypSmlouva", OracleDbType.Varchar2).Value = zamestnanec.TypSmlouva;
                    command.Parameters.Add("p_Plat", OracleDbType.Decimal).Value = zamestnanec.Plat;
                    command.Parameters.Add("p_Pohlavi", OracleDbType.Int32).Value = zamestnanec.Pohlavi;
                    command.Parameters.Add("p_IdAdresa", OracleDbType.Int32).Value = zamestnanec.IdAdresa;
                    command.Parameters.Add("p_IdOddeleni", OracleDbType.Int32).Value = zamestnanec.IdOddeleni;
                    command.Parameters.Add("p_IdRecZamestnanec", OracleDbType.Int32).Value = zamestnanec.IdRecZamestnanec;
                    command.Parameters.Add("p_Username", OracleDbType.Varchar2).Value = zamestnanec.Username;
                    command.Parameters.Add("p_Password", OracleDbType.Varchar2).Value = zamestnanec.Password;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddMaterial(Material material)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    connection.Open();
                    // var query = "INSERT INTO MATERIAL (ID_MATERIAL, NAZEV) VALUES (S_MATERIAL.nextval, :nazev)";

                    using (var command = new OracleCommand("INSERT_MATERIAL", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Nastavení typu na uloženou proceduru
                        command.Parameters.Add(new OracleParameter("nazev", material.Nazev));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Chyba při volání procedury INSERT_MATERIAL", ex);
            }
            
        }
        public void AddZeme(Zeme zeme)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new OracleCommand("INSERT_ZEME", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new OracleParameter("p_nazev", zeme.Nazev));
                        command.Parameters.Add(new OracleParameter("p_stupen_nebezpeci", zeme.StupenNebezpeci));
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding Zeme: {ex.Message}");

            }
        }

        public void AddFotografie(Fotografie fotografie)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                // var query = "INSERT INTO FOTOGRAFIE (ID_PREDMET, NAZEV, POPIS, ZANR, LICENCE) VALUES (:IdPredmet, :Nazev, :Popis, :Zanr, :Licence)";

                using (var command = new OracleCommand("INSERT_FOTOGRAFIE", connection))
                {
                    command.Parameters.Add("IdPredmet", OracleDbType.Int32).Value = fotografie.IdPredmet;
                    command.Parameters.Add("Nazev", OracleDbType.Varchar2).Value = fotografie.Nazev;
                    command.Parameters.Add("Popis", OracleDbType.Varchar2).Value = fotografie.Popis;
                    command.Parameters.Add("Zanr", OracleDbType.Varchar2).Value = fotografie.Zanr;
                    command.Parameters.Add("Licence", OracleDbType.Varchar2).Value = fotografie.Licence;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddObraz(Obraz obraz)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                //var query = "INSERT INTO OBRAZ (ID_PREDMET, NAZEV, POPIS, UMELECKY_STYL, MEDIUM) VALUES (:IdPredmet, :Nazev, :Popis, :UmeleckyStyl, :Medium)";

                using (var command = new OracleCommand("INSERT_OBRAZ", connection))
                {
                    command.Parameters.Add("IdPredmet", OracleDbType.Int32).Value = obraz.IdPredmet;
                    command.Parameters.Add("Nazev", OracleDbType.Varchar2).Value = obraz.Nazev;
                    command.Parameters.Add("Popis", OracleDbType.Varchar2).Value = obraz.Popis;
                    command.Parameters.Add("UmeleckyStyl", OracleDbType.Varchar2).Value = obraz.UmeleckyStyl;
                    command.Parameters.Add("Medium", OracleDbType.Varchar2).Value = obraz.Medium;

                    command.ExecuteNonQuery();
                }
            }
        }
        public void AddSocha(Socha socha)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO SOCHA (ID_PREDMET, NAZEV, POPIS, VAHA, TECHNIKA_TVORBY) VALUES (:IdPredmet, :Nazev, :Popis, :Vaha, :TechnikaTvorby)";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("IdPredmet", OracleDbType.Int32).Value = socha.IdPredmet;
                    command.Parameters.Add("Nazev", OracleDbType.Varchar2).Value = socha.Nazev;
                    command.Parameters.Add("Popis", OracleDbType.Varchar2).Value = socha.Popis;
                    command.Parameters.Add("Vaha", OracleDbType.Decimal).Value = socha.Vaha;
                    command.Parameters.Add("TechnikaTvorby", OracleDbType.Varchar2).Value = socha.TechnikaTvorby;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddObec(Obec obec)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (var command = new OracleCommand("INSERT_OBCE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parametry odpovídající proceduře INSERT_OBCE
                    command.Parameters.Add(new OracleParameter("p_nazev", obec.Nazev));
                    command.Parameters.Add(new OracleParameter("p_id_zeme", obec.IdZeme));

                    // Proveď příkaz
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddMuzeum(Muzeum muzeum)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (var command = new OracleCommand("INSERT_MUZEUM", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new OracleParameter("p_nazev", muzeum.Nazev));
                    command.ExecuteNonQuery();
                }
            }
        }



        //GET METODY

        public List<Muzeum> GetAllMuzea()
        {
            var muzeumList = new List<Muzeum>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT ID_MUZEUM, NAZEV FROM MUZEUM";

                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            muzeumList.Add(new Muzeum
                            {
                                IdMuzeum = reader.GetInt32(reader.GetOrdinal("ID_MUZEUM")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV"))
                            });
                        }
                    }
                }
            }

            return muzeumList;
        }

        public Muzeum GetMuzeumById(int id)
        {
            Muzeum muzeum = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT ID_MUZEUM, NAZEV FROM MUZEUM WHERE ID_MUZEUM = :Id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("Id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            muzeum = new Muzeum
                            {
                                IdMuzeum = reader.GetInt32(reader.GetOrdinal("ID_MUZEUM")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV"))
                            };
                        }
                    }
                }
            }

            return muzeum;
        }




        public List<Obec> GetAllObce()
        {
            var obceList = new List<Obec>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
            SELECT O.ID_OBEC, O.NAZEV, Z.NAZEV AS ZEME_NAZEV
            FROM OBEC O
            LEFT JOIN ZEME Z ON O.ID_ZEME = Z.ID_ZEME";

                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obceList.Add(new Obec
                            {
                                IdObec = reader.GetInt32(reader.GetOrdinal("ID_OBEC")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                                ZemeNazev = reader.GetString(reader.GetOrdinal("ZEME_NAZEV"))
                            });
                        }
                    }
                }
            }

            return obceList;
        }


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

        public List<Zamestnanec> SearchZamestnanci(string searchQuery)
        {
            var result = new List<Zamestnanec>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
        SELECT 
            ID_ZAMESTNANEC, JMENO, PRIJMENI, EMAIL, TELEFON, POZICE, ROLE,
            NAZEV_ADRESY, NAZEV_ODDELENI, RODNE_CISLO, DATUM_ZAMESTNANI, 
            TYP_SMLOUVA, PLAT, POHLAVI
        FROM ZAMESTNANEC_PRIVACY_VIEW
        WHERE LOWER(JMENO) LIKE '%' || :SearchQuery || '%'
           OR LOWER(PRIJMENI) LIKE '%' || :SearchQuery || '%'";


                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("SearchName", OracleDbType.Varchar2).Value = (searchQuery?? string.Empty).ToLower();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("ID_ZAMESTNANEC")),
                                Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                                Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                Telefon = reader.GetString(reader.GetOrdinal("TELEFON")),
                                Pozice = reader.GetString(reader.GetOrdinal("POZICE")),
                                Role = reader.GetString(reader.GetOrdinal("ROLE")),
                                AdresaText = reader.GetString(reader.GetOrdinal("NAZEV_ADRESY")),
                                OddeleniText = reader.GetString(reader.GetOrdinal("NAZEV_ODDELENI")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RODNE_CISLO")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DATUM_ZAMESTNANI")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TYP_SMLOUVA")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("PLAT")),
                                
                            });
                        }
                    }
                }
            }

            return result;
        }

        public List<Zamestnanec> GetZamestnanci(string searchQuery = null)
        {
            var zamestnanci = new List<Zamestnanec>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
           SELECT 
                ID_ZAMESTNANEC, JMENO, PRIJMENI, EMAIL, TELEFON, POZICE, ROLE,
                NAZEV_ADRESY, NAZEV_ODDELENI, RODNE_CISLO, DATUM_ZAMESTNANI,
                TYP_SMLOUVA, PLAT, POHLAVI
            FROM ZAMESTNANEC_PRIVACY_VIEW
            WHERE (:SearchQuery IS NULL OR LOWER(JMENO) LIKE '%' || :SearchQuery || '%'
                OR LOWER(PRIJMENI) LIKE '%' || :SearchQuery || '%')";


                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("SearchQuery", OracleDbType.Varchar2).Value = searchQuery?.ToLower() ?? (object)DBNull.Value;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zamestnanci.Add(new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("ID_ZAMESTNANEC")),
                                Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                                Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                Telefon = reader.GetString(reader.GetOrdinal("TELEFON")),
                                Pozice = reader.GetString(reader.GetOrdinal("POZICE")),
                                Role = reader.GetString(reader.GetOrdinal("ROLE")),
                                AdresaText = reader.GetString(reader.GetOrdinal("NAZEV_ADRESY")),
                                OddeleniText = reader.GetString(reader.GetOrdinal("NAZEV_ODDELENI")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RODNE_CISLO")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DATUM_ZAMESTNANI")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TYP_SMLOUVA")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("PLAT")),
                                PohlaviText = reader.IsDBNull(reader.GetOrdinal("POHLAVI"))
                            ? "Neuvedeno"
                            : reader.GetInt32(reader.GetOrdinal("POHLAVI")) == 1 ? "Muž" : "Žena"
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
                ID_ZAMESTNANEC, 
                POZICE, 
                JMENO, 
                PRIJMENI, 
                EMAIL, 
                TELEFON, 
                RODNE_CISLO, 
                DATUM_ZAMESTNANI, 
                TYP_SMLOUVA, 
                PLAT, 
                POHLAVI, 
                ID_ADRESA, 
                ID_ODDELENI, 
                ID_REC_ZAMESTNANEC,
                USERNAME, 
                PASSWORD,
                ROLE
            FROM ZAMESTNANEC
            WHERE USERNAME = :Username"; ;
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("username", username));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            zamestnanec = new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("ID_ZAMESTNANEC")),
                                Pozice = reader.GetString(reader.GetOrdinal("POZICE")),
                                Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                                Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                Telefon = reader.GetString(reader.GetOrdinal("TELEFON")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RODNE_CISLO")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DATUM_ZAMESTNANI")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TYP_SMLOUVA")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("PLAT")),
                                Pohlavi = reader.IsDBNull(reader.GetOrdinal("POHLAVI")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("POHLAVI")),
                                IdAdresa = reader.GetInt32(reader.GetOrdinal("ID_ADRESA")),
                                IdOddeleni = reader.GetInt32(reader.GetOrdinal("ID_ODDELENI")),
                                IdRecZamestnanec = reader.IsDBNull(reader.GetOrdinal("ID_REC_ZAMESTNANEC")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ID_REC_ZAMESTNANEC")),
                                Username = reader.IsDBNull(reader.GetOrdinal("USERNAME")) ? null : reader.GetString(reader.GetOrdinal("USERNAME")),
                                Password = reader.IsDBNull(reader.GetOrdinal("PASSWORD")) ? null : reader.GetString(reader.GetOrdinal("PASSWORD")),
                                Role = reader.IsDBNull(reader.GetOrdinal("ROLE")) ? null : reader.GetString(reader.GetOrdinal("ROLE"))
                            };
                        }
                    }
                }
            }
            return zamestnanec;
        }

        public Zamestnanec GetZamestnanecByName(string jmeno, string prijmeni)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM ZAMESTNANEC WHERE JMENO = :Jmeno AND PRIJMENI = :Prijmeni";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("Jmeno", OracleDbType.Varchar2).Value = jmeno;
                    command.Parameters.Add("Prijmeni", OracleDbType.Varchar2).Value = prijmeni;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("ID_ZAMESTNANEC")),
                                Username = reader.IsDBNull(reader.GetOrdinal("USERNAME")) ? null : reader.GetString(reader.GetOrdinal("USERNAME")),
                                Password = reader.IsDBNull(reader.GetOrdinal("PASSWORD")) ? null : reader.GetString(reader.GetOrdinal("PASSWORD")),
                                Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                                // další vlastnosti dle potřeby
                            };
                        }
                    }
                }
            }
            return null; // Pokud zaměstnanec s daným jménem a příjmením neexistuje
        }

        public Zamestnanec GetZamestnanecById(int idZamestnanec)
        {
            Zamestnanec zamestnanec = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                ID_ZAMESTNANEC, POZICE, JMENO, PRIJMENI, EMAIL, TELEFON, 
                RODNE_CISLO, DATUM_ZAMESTNANI, TYP_SMLOUVA, PLAT, POHLAVI, 
                ID_ADRESA, ID_ODDELENI, ID_REC_ZAMESTNANEC, USERNAME, PASSWORD, ROLE
            FROM ZAMESTNANEC
            WHERE ID_ZAMESTNANEC = :IdZamestnanec";
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("IdZamestnanec", idZamestnanec));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            zamestnanec = new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("ID_ZAMESTNANEC")),
                                Pozice = reader.GetString(reader.GetOrdinal("POZICE")),
                                Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                                Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                Telefon = reader.GetString(reader.GetOrdinal("TELEFON")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RODNE_CISLO")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DATUM_ZAMESTNANI")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TYP_SMLOUVA")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("PLAT")),
                                Pohlavi = reader.IsDBNull(reader.GetOrdinal("POHLAVI")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("POHLAVI")),
                                IdAdresa = reader.GetInt32(reader.GetOrdinal("ID_ADRESA")),
                                IdOddeleni = reader.GetInt32(reader.GetOrdinal("ID_ODDELENI")),
                                IdRecZamestnanec = reader.IsDBNull(reader.GetOrdinal("ID_REC_ZAMESTNANEC")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ID_REC_ZAMESTNANEC")),
                                Username = reader.IsDBNull(reader.GetOrdinal("USERNAME")) ? null : reader.GetString(reader.GetOrdinal("USERNAME")),
                                Password = reader.IsDBNull(reader.GetOrdinal("PASSWORD")) ? null : reader.GetString(reader.GetOrdinal("PASSWORD")),
                                Role = reader.IsDBNull(reader.GetOrdinal("ROLE")) ? null : reader.GetString(reader.GetOrdinal("ROLE"))
                            };
                        }
                    }
                }
            }
            return zamestnanec;
        }

        public Zamestnanec GetZamestnanecJoinDetails(string username)
        {
            Zamestnanec zamestnanec = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
            SELECT 
            z.ID_ZAMESTNANEC,
            z.POZICE,
            z.JMENO, 
            z.PRIJMENI, 
            z.EMAIL, 
            z.TELEFON,
            z.RODNE_CISLO,
            z.DATUM_ZAMESTNANI,
            z.TYP_SMLOUVA,
            z.PLAT,
            z.POHLAVI,
            a.ULICE || ', ' || a.PSC AS NAZEV_ADRESY,
            o.NAZEV AS NAZEV_ODDELENI,
            z.ROLE
        FROM 
            ZAMESTNANEC z
        LEFT JOIN ADRESA a ON z.ID_ADRESA = a.ID_ADRESA
        LEFT JOIN ODDELENI o ON z.ID_ODDELENI = o.ID_ODDELENI
        WHERE 
            z.USERNAME = :Username";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("Username", username));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Uložení dat do paměti
                            var idZamestnanec = reader.GetInt32(reader.GetOrdinal("ID_ZAMESTNANEC"));
                            var pozice = reader.GetString(reader.GetOrdinal("POZICE"));
                            var jmeno = reader.GetString(reader.GetOrdinal("JMENO"));
                            var prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI"));
                            var email = reader.GetString(reader.GetOrdinal("EMAIL"));
                            var telefon = reader.GetString(reader.GetOrdinal("TELEFON"));
                            var rodCislo = reader.GetString(reader.GetOrdinal("RODNE_CISLO"));
                            var datumZamestnani = reader.GetDateTime(reader.GetOrdinal("DATUM_ZAMESTNANI"));
                            var typSmlouva = reader.GetString(reader.GetOrdinal("TYP_SMLOUVA"));
                            var plat = reader.GetInt32(reader.GetOrdinal("PLAT"));
                            var adresaText = reader.IsDBNull(reader.GetOrdinal("NAZEV_ADRESY")) ? "Neznámá adresa" : reader.GetString(reader.GetOrdinal("NAZEV_ADRESY"));
                            var oddeleniText = reader.IsDBNull(reader.GetOrdinal("NAZEV_ODDELENI")) ? "Neznámé oddělení" : reader.GetString(reader.GetOrdinal("NAZEV_ODDELENI"));
                            var role = reader.GetString(reader.GetOrdinal("ROLE"));

                            // Naplnění objektu
                            zamestnanec = new Zamestnanec
                            {
                                IdZamestnanec = idZamestnanec,
                                Jmeno = jmeno,
                                Prijmeni = prijmeni,
                                Email = email,
                                Telefon = telefon,
                                Pozice = pozice,
                                Plat = plat,
                                TypSmlouva = typSmlouva,
                                RodCislo = rodCislo,
                                DatumZamestnani = datumZamestnani,
                                AdresaText = adresaText,
                                OddeleniText = oddeleniText,
                                Role = role
                            };
                        }
                    }
                }
            }
            return zamestnanec;
        }


        //pohled
        public Zamestnanec GetZamestnanecFromView(int id)
        {
            Zamestnanec zamestnanec = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = @"
           SELECT 
                ID_ZAMESTNANEC,
                JMENO,
                PRIJMENI,
                EMAIL,
                TELEFON,
                POZICE,
                ROLE, 
                NAZEV_ADRESY,
                NAZEV_ODDELENI,
                RODNE_CISLO,
                DATUM_ZAMESTNANI, 
                TYP_SMLOUVA,
                PLAT,
                POHLAVI
            FROM 
                ZAMESTNANEC_PRIVACY_VIEW
            WHERE
                ID_ZAMESTNANEC = :Id";
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("Id", OracleDbType.Int32).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            zamestnanec = new Zamestnanec
                            {
                                IdZamestnanec = reader.GetInt32(reader.GetOrdinal("ID_ZAMESTNANEC")),
                                Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                                Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                Telefon = reader.GetString(reader.GetOrdinal("TELEFON")),
                                Pozice = reader.GetString(reader.GetOrdinal("POZICE")),
                                Role = reader.GetString(reader.GetOrdinal("ROLE")),
                                AdresaText = reader.GetString(reader.GetOrdinal("NAZEV_ADRESY")),
                                OddeleniText = reader.GetString(reader.GetOrdinal("NAZEV_ODDELENI")),
                                RodCislo = reader.GetString(reader.GetOrdinal("RODNE_CISLO")),
                                DatumZamestnani = reader.GetDateTime(reader.GetOrdinal("DATUM_ZAMESTNANI")),
                                TypSmlouva = reader.GetString(reader.GetOrdinal("TYP_SMLOUVA")),
                                Plat = reader.GetDecimal(reader.GetOrdinal("PLAT")),
                                PohlaviText = reader.IsDBNull(reader.GetOrdinal("POHLAVI"))
    ? "Neuvedeno"
    : reader.GetInt32(reader.GetOrdinal("POHLAVI")) == 1 ? "Muž" : "Žena",
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

        public void InsertZamestnanec(Zamestnanec zamestnanec)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("INSERT_BALICEK.INSERT_ZAMESTNANEC", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_pozice", OracleDbType.Varchar2).Value = zamestnanec.Pozice ?? (object)DBNull.Value;
                    command.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = zamestnanec.Jmeno;
                    command.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = zamestnanec.Prijmeni;
                    command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = zamestnanec.Email;
                    command.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = zamestnanec.Telefon ?? (object)DBNull.Value;
                    command.Parameters.Add("p_rodne_cislo", OracleDbType.Varchar2).Value = zamestnanec.RodCislo ?? (object)DBNull.Value;
                    command.Parameters.Add("p_datum_zamestnani", OracleDbType.Date).Value = zamestnanec.DatumZamestnani;
                    command.Parameters.Add("p_typ_smlouva", OracleDbType.Varchar2).Value = zamestnanec.TypSmlouva ?? (object)DBNull.Value;
                    command.Parameters.Add("p_plat", OracleDbType.Decimal).Value = zamestnanec.Plat;
                    command.Parameters.Add("p_pohlavi", OracleDbType.Int32).Value = zamestnanec.Pohlavi.HasValue ? (object)zamestnanec.Pohlavi.Value : DBNull.Value; // Změněno na Int32
                    command.Parameters.Add("p_id_adresa", OracleDbType.Int32).Value = zamestnanec.IdAdresa;
                    command.Parameters.Add("p_id_oddeleni", OracleDbType.Int32).Value = zamestnanec.IdOddeleni;
                    command.Parameters.Add("p_id_rec_zamestnanec", OracleDbType.Int32).Value = zamestnanec.IdRecZamestnanec.HasValue ? (object)zamestnanec.IdRecZamestnanec.Value : DBNull.Value;
                    command.Parameters.Add("p_username", OracleDbType.Varchar2).Value = zamestnanec.Username;
                    command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = zamestnanec.Password;
                    command.Parameters.Add("p_role", OracleDbType.Varchar2).Value = zamestnanec.Role;

                    command.ExecuteNonQuery();
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

        public Obec GetObecById(int id)
        {
            Obec obec = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                var query = @"
            SELECT 
                ID_OBEC, 
                NAZEV, 
                ID_ZEME
            FROM OBEC
            WHERE ID_OBEC = :id";

                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            obec = new Obec
                            {
                                IdObec = reader.GetInt32(reader.GetOrdinal("ID_OBEC")),
                                Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                                IdZeme = reader.GetInt32(reader.GetOrdinal("ID_ZEME"))
                            };
                        }
                    }
                }
            }

            return obec;
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

                    using (var reader = command.ExecuteReader())
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
        public void UpdateObec(Obec obec)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (var command = new OracleCommand("UPDATE_OBEC", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new OracleParameter("p_id_obec", obec.IdObec));
                    command.Parameters.Add(new OracleParameter("p_nazev", obec.Nazev));
                    command.Parameters.Add(new OracleParameter("p_id_zeme", obec.IdZeme));

                    command.ExecuteNonQuery();
                }
            }
        }


        public void UpdateZamestnanec(Zamestnanec zamestnanec)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();


                using (var command = new OracleCommand("UPDATE_BALICEK.UPDATE_ZAMESTNANEC", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_IdZamestnanec", OracleDbType.Int32).Value = zamestnanec.IdZamestnanec;
                    command.Parameters.Add("p_Pozice", OracleDbType.Varchar2).Value = zamestnanec.Pozice;
                    command.Parameters.Add("p_Jmeno", OracleDbType.Varchar2).Value = zamestnanec.Jmeno;
                    command.Parameters.Add("p_Prijmeni", OracleDbType.Varchar2).Value = zamestnanec.Prijmeni;
                    command.Parameters.Add("p_Email", OracleDbType.Varchar2).Value = zamestnanec.Email;
                    command.Parameters.Add("p_Telefon", OracleDbType.Varchar2).Value = zamestnanec.Telefon;
                    command.Parameters.Add("p_RodneCislo", OracleDbType.Varchar2).Value = zamestnanec.RodCislo;
                    command.Parameters.Add("p_DatumZamestnani", OracleDbType.Date).Value = zamestnanec.DatumZamestnani;
                    command.Parameters.Add("p_TypSmlouva", OracleDbType.Varchar2).Value = zamestnanec.TypSmlouva;
                    command.Parameters.Add("p_Plat", OracleDbType.Decimal).Value = zamestnanec.Plat;
                    command.Parameters.Add("p_Pohlavi", OracleDbType.Int32).Value = zamestnanec.Pohlavi.HasValue ? (object)zamestnanec.Pohlavi.Value : DBNull.Value;
                    command.Parameters.Add("p_IdAdresa", OracleDbType.Int32).Value = zamestnanec.IdAdresa;
                    command.Parameters.Add("p_IdOddeleni", OracleDbType.Int32).Value = zamestnanec.IdOddeleni;
                    command.Parameters.Add("p_IdRecZamestnanec", OracleDbType.Int32).Value = zamestnanec.IdRecZamestnanec ?? (object)DBNull.Value;
                    command.Parameters.Add("p_Username", OracleDbType.Varchar2).Value = zamestnanec.Username ?? (object)DBNull.Value;
                    command.Parameters.Add("p_Password", OracleDbType.Varchar2).Value = zamestnanec.Password ?? (object)DBNull.Value;
                    command.Parameters.Add("p_Role", OracleDbType.Varchar2).Value = zamestnanec.Role;



                    command.ExecuteNonQuery();
                }

            }
        }

        public void UpdateZamestnanecLoginData(int idZamestnanec, string role, string username, string password)
        {
            Zamestnanec zamestnanec = GetZamestnanecById(idZamestnanec);
            if (zamestnanec != null)
            {
                zamestnanec.Role = role ?? zamestnanec.Role;
                zamestnanec.Username = username ?? zamestnanec.Username;
                zamestnanec.Password = password ?? zamestnanec.Password;
            }
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("UPDATE_BALICEK.UPDATE_ZAMESTNANEC", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Předání všech parametrů do procedury, stávající hodnoty se ponechají tam, kde nebyla změna
                    command.Parameters.Add("p_id_zamestnanec", OracleDbType.Int32).Value = zamestnanec.IdZamestnanec;
                    command.Parameters.Add("p_pozice", OracleDbType.Varchar2).Value = zamestnanec.Pozice;
                    command.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = zamestnanec.Jmeno;
                    command.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = zamestnanec.Prijmeni;
                    command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = zamestnanec.Email;
                    command.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = zamestnanec.Telefon;
                    command.Parameters.Add("p_rodne_cislo", OracleDbType.Varchar2).Value = zamestnanec.RodCislo;
                    command.Parameters.Add("p_datum_zamestnani", OracleDbType.Date).Value = zamestnanec.DatumZamestnani;
                    command.Parameters.Add("p_typ_smlouva", OracleDbType.Varchar2).Value = zamestnanec.TypSmlouva;
                    command.Parameters.Add("p_plat", OracleDbType.Decimal).Value = zamestnanec.Plat;
                    command.Parameters.Add("p_pohlavi", OracleDbType.Int32).Value = zamestnanec.Pohlavi.HasValue ? (object)zamestnanec.Pohlavi.Value : DBNull.Value;
                    command.Parameters.Add("p_id_adresa", OracleDbType.Int32).Value = zamestnanec.IdAdresa;
                    command.Parameters.Add("p_id_oddeleni", OracleDbType.Int32).Value = zamestnanec.IdOddeleni;
                    command.Parameters.Add("p_id_rec_zamestnanec", OracleDbType.Int32).Value = zamestnanec.IdRecZamestnanec.HasValue ? (object)zamestnanec.IdRecZamestnanec.Value : DBNull.Value;
                    command.Parameters.Add("p_username", OracleDbType.Varchar2).Value = zamestnanec.Username ?? (object)DBNull.Value;
                    command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = zamestnanec.Password ?? (object)DBNull.Value;
                    command.Parameters.Add("p_role", OracleDbType.Varchar2).Value = zamestnanec.Role ?? (object)DBNull.Value;

                    command.ExecuteNonQuery();
                }
            }
        }


        public void UpdateZeme(Zeme zeme)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                //var query = "UPDATE ZEME SET NAZEV = :nazev, STUPEN_NEBEZPECI = :stupenNebezpeci WHERE ID_ZEME = :id";

                using (var command = new OracleCommand("UPDATE_ZEME", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new OracleParameter("p_id_zeme", zeme.IdZeme));
                    command.Parameters.Add(new OracleParameter("p_nazev", zeme.Nazev));
                    command.Parameters.Add(new OracleParameter("p_stupen_nebezpeci", zeme.StupenNebezpeci));
                    
                    command.ExecuteNonQuery();
                }
            }
        }


        public void UpdateMaterial(Material material)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                // var query = "UPDATE MATERIAL SET NAZEV = :nazev WHERE ID_MATERIAL = :id";

                using (var command = new OracleCommand("UPDATE_MATERIAL", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new OracleParameter("nazev", material.Nazev));
                    command.Parameters.Add(new OracleParameter("id", material.IdMaterial));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMuzeum(Muzeum muzeum)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (var command = new OracleCommand("UPDATE_MUZEUM", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new OracleParameter("p_id_muzeum", muzeum.IdMuzeum));
                    command.Parameters.Add(new OracleParameter("p_nazev", muzeum.Nazev));
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
                //var query = "DELETE FROM MATERIAL WHERE ID_MATERIAL = :id";

                using (var command = new OracleCommand("DELETE_MATERIAL", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
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
                //var query = "DELETE FROM ZEME WHERE ID_ZEME = :id";

                using (var command = new OracleCommand("DELETE_ZEME", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new OracleParameter("id", id));
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteMuzeum(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (var command = new OracleCommand("DELETE_MUZEUM", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new OracleParameter("p_id_muzeum", id));
                    command.ExecuteNonQuery();
                }
            }
        }


        //pro systémový katalog
        public List<string> GetUserTables()
        {
            var tables = new List<string>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT TABLE_NAME FROM USER_TABLES";
                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return tables;
        }

        public List<string> GetUserTriggers()
        {
            var triggers = new List<string>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT TRIGGER_NAME FROM USER_TRIGGERS";
                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            triggers.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return triggers;
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
