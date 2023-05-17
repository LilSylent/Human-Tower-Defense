using TMPro;
using UnityEngine;

public class StatisztikaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eletText;
    [SerializeField] private TextMeshProUGUI penzText;

    private void Update()
    {
        eletText.text = $"{JatekosStatisztika.JelenlegiElet}";
        penzText.text = $"${JatekosStatisztika.JelenlegiPenz}";
    }
}
