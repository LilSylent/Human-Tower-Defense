using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerPrefsBetoltes : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private bool teljesKepernyos;
    private static string mainMenu = "MainMenu";

    private void Awake()
    {
        switch (PlayerPrefs.GetInt("teljesKepernyos", 0))
        {
            case 0:
                teljesKepernyos = false;
                break;
            case 1:
                teljesKepernyos = true;
                break;
        }

        Screen.fullScreen = teljesKepernyos;

        //Beállítjuk a már beállított minőségre a játékot amennyiben van, ha pedig nincs akkor legrosszabb minőségre tesszük.
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("minoseg", 0));

        mixer.SetFloat("MasterHang", Mathf.Log10(PlayerPrefs.GetFloat("foHangero", 1f)) * 20);
        mixer.SetFloat("EffektHang", Mathf.Log10(PlayerPrefs.GetFloat("effektHangero", 0.5f)) * 20);
        mixer.SetFloat("ZeneHang", Mathf.Log10(PlayerPrefs.GetFloat("zeneHangero", 0.25f)) * 20);

        try
        {
            //Ellenőrizzük, hogy a felhasználó már belépett-e (Ilyenkor van nev kulcsa), és ellenőrizzük azt is, hogy az a név még létezik e (Mivel lehet, hogy az adatbázisból már ki lett törölve), ha ez igaz, akkor...
            if (PlayerPrefs.HasKey("nev") && Felhasznalokezelo.Db.LetezoFelhasznalo(PlayerPrefs.GetString("nev")))
            {
                    //Átrakjuk a játékost egyből a MainMenübe
                    SceneManager.LoadScene(mainMenu);
            }
            else
            {
                //Amúgy meg töröljük a kulcsokat
                PlayerPrefs.DeleteKey("nev");
                PlayerPrefs.DeleteKey("uid");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}
