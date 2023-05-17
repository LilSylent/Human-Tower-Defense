using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text nevText;

    [Header("Pályaválasztás")]
    [SerializeField] private Button[] palyaGombok;
    private string palyanev;

    [Header("Karrier")]
    [SerializeField] private TMP_Text osszesJatekText;
    [SerializeField] private TMP_Text nyertJatekText;
    [SerializeField] private TMP_Text vesztettJatekText;

    private void Awake()
    {
        nevText.enabled = false;
        try
        {
            //Ha bevagyunk lépve, akkor...
            if (PlayerPrefs.HasKey("nev") && Felhasznalokezelo.Db.LetezoFelhasznalo(PlayerPrefs.GetString("nev")))
            {
                //Kiírjuk a játékos nevét
                nevText.text = $"Üdv, {PlayerPrefs.GetString("nev")}";
                nevText.enabled = true;
            }
            else
            {
                //Különben áttöltjük a játékost a Log/Reg scene-re
                SceneManager.LoadScene(0);
                //nevText.enabled = false;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void Start()
    {
        //PÁLYA
        //Megszerezzük az elért szintünket
        int elertSzint = PlayerPrefs.GetInt("elertSzint", 1);

        //Végigmegyünk a gombokon
        for (int i = 0; i < palyaGombok.Length; i++)
        {
            //Ha az i+1 > elertSzint, akkor... (pl: i=0 | 0+1 = 1 | 1 > 1 = nem)
            if (i + 1 > elertSzint)
            {
                //Azt a gombot nem lehet megnyomni
                palyaGombok[i].interactable = false;
            }
        }

        //KARRIER
        try
        {
            //Megszerezzük a nyert és vesztett játékokat
            List<bool> karrierLista = Felhasznalokezelo.Db.JatekokMegszerzese(PlayerPrefs.GetInt("uid"));

            //Átadjuk a szövegnek a lista hosszát
            osszesJatekText.text = $"{karrierLista.Count}";
            //Átadjuk a szövegnek a lista igaz elemeinek darabszámát
            nyertJatekText.text = $"{karrierLista.Count(x => x == true)}";
            //Átadjuk a szövegnek a lista hamis elemeinek darabszámát
            vesztettJatekText.text = $"{karrierLista.Count(x => x == false)}";

            //Töröljük a listában lévő adatokat
            karrierLista.Clear();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public void Kilepes()
    {
        //Kiírjuk konzolra, mivel editor-on belül nem működik az Application.Quit();
        Debug.Log("Kilépés");
        //Bezárjuk a programot
        Application.Quit();
    }

    public void Kijelentkezes()
    {
        //Töröljük a kulcsokat
        PlayerPrefs.DeleteKey("nev");
        PlayerPrefs.DeleteKey("uid");

        //Átrakjuk a LogReg felületre
        SceneManager.LoadScene(0);
    }

    public void MapValasztas(string palya)
    {
        //Átadjuk a pályát a változónak
        palyanev = palya;
    }

    public void MapBetoltes(int nehezseg)
    {
        //Beállítjuk a gomb által átadott nehézséget
        Nehesegvezerlo.Nehezseg = (Nehezsegek)nehezseg;
        //Betöltjük a pályát
        SceneManager.LoadScene(palyanev);
    }
}
