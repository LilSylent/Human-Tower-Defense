using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KorUI : MonoBehaviour
{
    [SerializeField] private TMP_Text korText;

    private void Update()
    {
        korText.text = $"Kör: {JatekosStatisztika.JelenlegiKor} / {KorSpawner.MaxKor}";
    }
}
