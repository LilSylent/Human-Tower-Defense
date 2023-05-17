using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BeallitasKezelo : MonoBehaviour
{
    #region Változók
    [Header("Kép")]
    [SerializeField] private TMP_Dropdown felbontasDropdown;
    [SerializeField] private TMP_Dropdown minosegDropdown;
    [SerializeField] private Toggle teljesKepernyosToggle;

    private bool teljesKepernyos;
    private int minosegiSzint;
    private Resolution felbontas;
    private Resolution[] felbontasok;

    [Header("Hang")]
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider foHangeroSlider;
    [SerializeField] private TMP_Text foHangeroText;

    [SerializeField] private Slider effektHangeroSlider;
    [SerializeField] private TMP_Text effektHangeroText;

    [SerializeField] private Slider zeneHangeroSlider;
    [SerializeField] private TMP_Text zeneHangeroText;

    private float foHangero;
    private float effektHangero;
    private float zeneHangero;
    #endregion

    private void Awake()
    {
        //TELJES KÉPERNYŐS BETÖLTÉS
        switch (PlayerPrefs.GetInt("teljesKepernyos", 0))
        {
            case 0:
                teljesKepernyos = false;
                break;
            case 1:
                teljesKepernyos = true;
                break;
        }

        teljesKepernyosToggle.isOn = teljesKepernyos;

        //MINŐSÉG BETÖLTÉS
        minosegiSzint = PlayerPrefs.GetInt("minoseg", 0);
        minosegDropdown.value = minosegiSzint;

        //HANGERŐ BETÖLTÉS
        foHangeroSlider.value = PlayerPrefs.GetFloat("foHangero", 1f);
        effektHangeroSlider.value = PlayerPrefs.GetFloat("effektHangero", 0.5f);
        zeneHangeroSlider.value = PlayerPrefs.GetFloat("zeneHangero", 0.25f);
    }

    private void Start()
    {
        //FELBONTÁS BEÁLLÍTÁS

        //Az elérhető felbontásokat letároljuk
        felbontasok = Screen.resolutions;
        //Töröljük az alapértékeket a legördülő listából
        felbontasDropdown.ClearOptions();

        List<string> formazottFelbontasok = new List<string>();

        int jelenlegiFelbontasIndex = 0;
        //Végigmegyünk a létező felbontásokon
        for (int i = 0; i < felbontasok.Length; i++)
        {
            //Megformázzuk a felbontást
            string temp = $"{felbontasok[i].width} x {felbontasok[i].height} {felbontasok[i].refreshRate} Hz";
            //Azt hozzáadjuk a tömbhöz
            formazottFelbontasok.Add(temp);

            //Ha megegyezik a felbontás a jelenlegi felbontásunkkal, akkor...
            if (felbontasok[i].width == Screen.width && felbontasok[i].height == Screen.height)
            {
                //Letároljuk annak az indexét
                jelenlegiFelbontasIndex = i;
            }
        }

        //Betöltjük a felbontásokat
        felbontasDropdown.AddOptions(formazottFelbontasok);
        //Kiválasztjuk a jelenlegi felbontásunkat
        felbontasDropdown.value = jelenlegiFelbontasIndex;
        //Újratöltjük a megjelenítést
        felbontasDropdown.RefreshShownValue();
    }

    public void FelbontasBeallitas(int felbontasIndex)
    {
        felbontas = felbontasok[felbontasIndex];
    }

    public void MinosegBeallitas(int minosegIndex)
    {
        minosegiSzint = minosegIndex;
    }

    public void TeljesKepernyoBeallitas(bool _teljesKepernyos)
    {
        teljesKepernyos = _teljesKepernyos;
    }

    public void FoHangBeallitas(float hangero)
    {
        mixer.SetFloat("MasterHang", Mathf.Log10(hangero) * 20);
        foHangero = hangero;
        foHangeroText.text = string.Format("{0:0.00}", foHangero);
    }

    public void EffektHangBeallitas(float hangero)
    {
        mixer.SetFloat("EffektHang", Mathf.Log10(hangero) * 20);
        effektHangero = hangero;
        effektHangeroText.text = string.Format("{0:0.00}", effektHangero);
    }

    public void ZeneHangBeallitas(float hangero)
    {
        mixer.SetFloat("ZeneHang", Mathf.Log10(hangero) * 20);
        zeneHangero = hangero;
        zeneHangeroText.text = string.Format("{0:0.00}", zeneHangero);
    }

    public void BeallitasAlkalmazas()
    {
        //Lementjük a Minőség preferenciát PlayerPrefs-be
        PlayerPrefs.SetInt("minoseg", minosegiSzint);
        //Beállítjuk a minőséget
        QualitySettings.SetQualityLevel(minosegiSzint);

        //Lementjük a Teljes képernyőt preferenciát PlayerPrefs-be
        PlayerPrefs.SetInt("teljesKepernyos", teljesKepernyos ? 1 : 0);
        //Beállítjuk, hogy teljes képernyős-e vagy sem
        Screen.fullScreen = teljesKepernyos;

        //Beállítjuk a felbontást
        Screen.SetResolution(felbontas.width, felbontas.height, teljesKepernyos);

        //Lementjük a Hangerő preferenciákat PlayerPrefs-be
        PlayerPrefs.SetFloat("foHangero", foHangero);
        PlayerPrefs.SetFloat("effektHangero", effektHangero);
        PlayerPrefs.SetFloat("zeneHangero", zeneHangero);
    }
}
