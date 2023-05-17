using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Jatekvezerlo : MonoBehaviour
{
    private static string nehezseg;
    [SerializeField] private int kovetkezoSzint = 2;
    private static bool vegetErt;

    [SerializeField] private GameObject jatekVegeUI;
    [SerializeField] private GameObject nyeresUI;

    public static string Nehezseg { get => nehezseg; set => nehezseg = value; }
    public static bool VegetErt { get => vegetErt; set => vegetErt = value; }

    private void Start()
    {
        Time.timeScale = 1f;
        vegetErt = false;
    }

    private void Update()
    {
        //Ha a jelenlegi életünk <= mint 0 és nem ért véget a játék akkor...
        if (JatekosStatisztika.JelenlegiElet <= 0 && !vegetErt)
        {
            //Meghívjuk ezt
            JatekVege();
        }
    }

    private void JatekVege()
    {
        vegetErt = true;

        //Megjelenítjük a játék vége UI-t
        jatekVegeUI.SetActive(true);

        //Rögzitjűk a vesztést AB-ban
        Adatrogzites(false);

        //Megállítjuk az időt
        Time.timeScale = 0f;
    }

    public void Nyeres()
    {
        vegetErt = true;

        //Megjelenítjük a nyerés UI-t
        nyeresUI.SetActive(true);

        //Ha a következő szint nagyobb, mint amit megszerzünk a PlayerPrefs-ből, akkor...
        if (kovetkezoSzint > PlayerPrefs.GetInt("elertSzint"))
        {
            //Átírjuk a PlayerPrefs-ben lévő szintet
            PlayerPrefs.SetInt("elertSzint", kovetkezoSzint);
        }

        //Rögzitjűk a nyerést AB-ban
        Adatrogzites(true);

        //Megállítjuk az időt
        Time.timeScale = 0f;
    }

    private void Adatrogzites(bool nyert)
    {
        //Ha van név kulcsunk, akkor...
        if (PlayerPrefs.HasKey("nev"))
        {
            try
            {
                //Ha létezik a felhasználó, akkor...
                if (Felhasznalokezelo.Db.LetezoFelhasznalo(PlayerPrefs.GetString("nev")))
                {
                    //Beszúrjuk, hogy nyert vagy veszített a játékos
                    Felhasznalokezelo.Db.JatekRogzites(PlayerPrefs.GetInt("uid"), nyert);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Nincs név a PlayerPrefs-ben");
        }
    }

    public static void Ujrakezdes()
    {
        //Az időt visszaállítjuk normálisra
        Time.timeScale = 1f;
        //Újratöltjük a jelenlegi pályát
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void Menu()
    {
        //Az időt visszaállítjuk normálisra
        Time.timeScale = 1f;
        //Betöltjük a menüt
        SceneManager.LoadScene("MainMenu");
    }
}
