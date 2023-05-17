using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Felhasznalokezelo : MonoBehaviour
{
    private static string mainMenu = "MainMenu";

    [SerializeField] private TMP_Text hiba;

    [Header("Belépés")]
    [SerializeField] private TMP_InputField Log_felhasznalonev;
    [SerializeField] private TMP_InputField Log_jelszo;

    [Header("Regisztrálás")]
    [SerializeField] private TMP_InputField Reg_felhasznalonev;
    [SerializeField] private TMP_InputField Reg_jelszo;
    [SerializeField] private TMP_InputField Reg_jelszoUjra;

    private static DbConnect db = new DbConnect("127.0.0.1", "htd", "root", "");

    public static DbConnect Db { get => db; set => db = value; }

    private void Start()
    {
        //Indításkor a hiba text szövegét üresre állítjuk
        hiba.text = string.Empty;
    }

    public void Belepes()
    {
        //Ha minden ki van töltve, akkor...
        if (Log_felhasznalonev.text != string.Empty &&
            Log_jelszo.text != string.Empty)
        {
            //Elkapjuk a kivételt
            try
            {
                //Ha sikeres a belépés, akkor
                if (db.Belepes(Log_felhasznalonev.text, Log_jelszo.text))
                {
                    //PlayerPrefs-be letároljuk a felhasználó nevét
                    PlayerPrefs.SetString("nev", Log_felhasznalonev.text);

                    Siker();
                }
                else
                {
                    //Sikertelen belépéskor, kiírjuk a felhasználónak
                    hiba.text = "Hibás felhasználónév vagy jelszó";
                }
            }
            catch (System.Exception ex)
            {
                //Ha a kivétel az, hogy SikertelenKapcsolodas, akkor...
                if (ex.Message == "SikertelenKapcsolodas")
                {
                    //Kiírjuk a felhasználónak
                    hiba.text = "Nem sikerült csatlakozni az adatbázishoz!";
                }
            }
        }
        else
        {
            //Ha nincs minden kitöltve, kiírjuk a felhasználónak
            hiba.text = "Az összes mezőt kötelező kitölteni!";
        }
    }

    public void Regisztralas()
    {
        if (Reg_felhasznalonev.text != string.Empty &&
            Reg_jelszo.text != string.Empty &&
            Reg_jelszoUjra.text != string.Empty)
        {
            //Ha a két jelszó megegyezik, akkor...
            if (Reg_jelszo.text == Reg_jelszoUjra.text)
            {
                //Elkapjuk a kivételt
                try
                {
                    //Ha sikeres a regisztálás, akkor...
                    if (db.Regisztralas(Reg_felhasznalonev.text, Reg_jelszo.text))
                    {
                        //PlayerPrefs-be letároljuk a felhasználó nevét
                        PlayerPrefs.SetString("nev", Reg_felhasznalonev.text);

                        Siker();
                    }
                    else
                    {
                        //Különben, kiírjuk, hogy van ilyen felhasználó
                        hiba.text = "A felhasználó már létezik!";
                    }
                }
                catch (System.Exception ex)
                {
                    //Ha a kivétel az, hogy SikertelenKapcsolodas, akkor...
                    if (ex.Message == "SikertelenKapcsolodas")
                    {
                        //Kiírjuk a felhasználónak
                        hiba.text = "Nem sikerült csatlakozni az adatbázishoz!";
                    }
                }
            }
            else
            {
                //Ha nem egyezik kiírjuk
                hiba.text = "A jelszavak nem egyeznek!";
            }
        }
        else
        {
            //Ha nincs minden kitöltve kiírjuk
            hiba.text = "Az összes mezőt kötelező kitölteni!";
        }
    }

    private void Siker()
    {
        int id = -1;

        //Megszerezzük a felhasználó ID-ját
        try
        {
            id = db.UserIDMegszerzes(PlayerPrefs.GetString("nev"));
        }
        catch (System.Exception ex)
        {
            if (ex.Message == "SikertelenKapcsolodas")
            {
                //Kiírjuk a felhasználónak
                hiba.text = "Nem sikerült csatlakozni az adatbázishoz!";
            }
        }

        if (id != -1)
        {
            PlayerPrefs.SetInt("uid", id);
        }

        //Átrakjuk a MainMenu scene-re
        SceneManager.LoadScene(mainMenu);
    }
}
