using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class DbConnect
{
    private MySqlConnection con;

    public DbConnect(string host, string dbname, string ui, string pw)
    {
        con = new MySqlConnection($"Database = {dbname}; Data Source = {host}; User Id = {ui}; Password = {pw};");
    }

    private bool Connect()
    {
        try { con.Open(); return true; }
        catch (Exception) { return false; }
    }
    private bool Connect_Close()
    {
        try { con.Close(); return true; }
        catch (Exception) { return false; }
    }

    public bool Belepes(string nev, string jelszo)
    {
        if (Connect())
        {
            bool van = false;
            string query = "SELECT nev FROM felhasznalok WHERE nev LIKE @nev AND jelszo LIKE @jelszo";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nev", nev);
            cmd.Parameters.AddWithValue("@jelszo", SHA256Szamolas(jelszo));
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                van = true;
            }

            reader.Close();
            Connect_Close();

            return van;
        }
        else
        {
            throw new Exception("SikertelenKapcsolodas");
        }
    }

    public bool Regisztralas(string nev, string jelszo)
    {
        bool letezik = false;

        try
        {
            letezik = LetezoFelhasznalo(nev);
        }
        catch (Exception){}

        if (Connect())
        {
            bool eredmeny = false;
            if (!letezik)
            {
                string query = "INSERT INTO felhasznalok(nev, jelszo) VALUES (@nev, @jelszo)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@nev", nev);
                cmd.Parameters.AddWithValue("@jelszo", SHA256Szamolas(jelszo));

                //Ha sikerült a hozzáadás, akkor igaz, különben hamis
                eredmeny = cmd.ExecuteNonQuery() > 0;
            }

            Connect_Close();
            return eredmeny;
        }
        else
        {
            throw new Exception("SikertelenKapcsolodas");
        }
    }

    public int UserIDMegszerzes(string nev)
    {
        if (Connect())
        {
            int eredmeny = -1;

            string query = "SELECT id FROM felhasznalok WHERE nev LIKE @nev";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nev", nev);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                eredmeny = reader.GetInt32(0);
            }

            reader.Close();
            Connect_Close();

            return eredmeny;
        }
        else
        {
            throw new Exception("SikertelenKapcsolodas");
        }
    }

    public bool LetezoFelhasznalo(string nev)
    {
        if (Connect())
        {
            bool letezik;
            string query = "SELECT nev FROM felhasznalok WHERE nev = @nev";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nev", nev);
            MySqlDataReader reader = cmd.ExecuteReader();

            letezik = reader.HasRows;

            reader.Close();
            Connect_Close();

            return letezik;
        }
        else
        {
            throw new Exception("SikertelenKapcsolodas");
        }
    }

    public void JatekRogzites(int id, bool nyert)
    {
        if (Connect())
        {
            string query = "INSERT INTO jatekok(felhasznaloId, nyert) VALUES (@id, @eredmeny)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@eredmeny", nyert);

            cmd.ExecuteNonQuery();

            Connect_Close();
        }
        else
        {
            throw new Exception("SikertelenKapcsolodas");
        }
    }

    public List<bool> JatekokMegszerzese(int id)
    {
        if (Connect())
        {
            List<bool> temp = new List<bool>();

            string query = "SELECT nyert FROM jatekok WHERE felhasznaloId = @id";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                temp.Add(reader.GetBoolean(0));
            }

            reader.Close();
            Connect_Close();

            return temp;
        }
        else
        {
            throw new Exception("SikertelenKapcsolodas");
        }
    }

    public string SHA256Szamolas(string szoveg)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // A beviteli mezőt átkonvertáljuk byte-okká, és abból hasht számítunk
            byte[] adat = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(szoveg));

            // Létrehozunk egy StringBuilder-t a byte-ok összegyüjtéséhez
            StringBuilder builder = new StringBuilder();

            //Az adat tömb minden egyes byte-ján végigmegyünk és azt egy hexadecimális karakterlánccá formázzuk
            for (int i = 0; i < adat.Length; i++)
            {
                builder.Append(adat[i].ToString("x2"));
            }

            //Visszaadjuk a hexadecimális string-et
            return builder.ToString();
        }
    }
}
