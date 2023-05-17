using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToronyUI : MonoBehaviour
{
    #region Változók
    private Torony torony;

    [SerializeField] private GameObject UI;

    [SerializeField] private TextMeshProUGUI nevText;
    [SerializeField] private Image ikonImage;

    [SerializeField] private Button fejlesztesGomb;
    [SerializeField] private TextMeshProUGUI fejlesztesText;

    [SerializeField] private TextMeshProUGUI eladasText;
    #endregion

    public void Update()
    {
        //Ha van tornyunk és az nincs fejlesztve
        if (torony != null && !torony.Fejlesztve)
        {
            //És több pénzünk van mint amennyibe a fejlesztés kerül, akkor...
            if (JatekosStatisztika.JelenlegiPenz >= torony.ToronyBlueprint.FejlesztesiAr)
            {
                //Bekapcsoljuk a gombot
                fejlesztesGomb.interactable = true;
            }
            else
            {
                //Kikapcsoljuk a gombot
                fejlesztesGomb.interactable = false;
            }
        }
    }

    public void ToronyBeallitas(Torony t)
    {
        torony = t;

        //Ha a torony fejlesztve van, akkor...
        if (torony.Fejlesztve)
        {
            //Beállítjuk a nevét
            nevText.text = $"{torony.ToronyBlueprint.Nev}\n2. szint";

            //Beállítjuk a ikonját
            ikonImage.sprite = torony.ToronyBlueprint.FejlesztettIkon;

            //Kikapcsoljuk a fejlesztés gombot
            fejlesztesGomb.interactable = false;

            //Átírjuk a fejlesztés gombban lévő szöveget.
            fejlesztesText.text = "MAX";
        }
        else
        {
            //Beállítjuk a nevét
            nevText.text = $"{torony.ToronyBlueprint.Nev}\n1. szint";

            //Beállítjuk a ikonját
            ikonImage.sprite = torony.ToronyBlueprint.Ikon;

            //Bekapcsoljuk a fejlesztés gombot
            fejlesztesGomb.interactable = true;

            //Átírjuk a fejlesztés gombban lévő szöveget.
            fejlesztesText.text = $"Fejlesztés\n${torony.ToronyBlueprint.FejlesztesiAr}";
        }

        //Beállítjuk, hogy mennyiért tudjuk eladni a tornyot
        eladasText.text = $"Eladás\n${torony.ToronyBlueprint.eladasiAr(t.Fejlesztve)}";

        UI.SetActive(true);
    }

    /// <summary>Ezzel tudjuk elrejteni a ToronyUI-t</summary>
    public void Elrejtes()
    {
        UI.SetActive(false);
    }

    /// <summary>Ezzel tudjuk fejleszteni a tornyunkat</summary>
    public void Fejlesztes()
    {
        torony.Kezelo.ToronyFejlesztes();
    }

    /// <summary>Ezzel tudjuk eladni a tornyunkat</summary>
    public void Eladas()
    {
        torony.Kezelo.ToronyEladas();
    }
}
