using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JatekVege : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI korText;

    //Akkor hívjuk meg amikor bekapcsoljuk a UI-t
    private void OnEnable()
    {
        korText.text = $"{JatekosStatisztika.JelenlegiKor}. kör";
    }

    public void Ujrakezdes()
    {
        Jatekvezerlo.Ujrakezdes();
    }

    public void Menu()
    {
        Jatekvezerlo.Menu();
    }

}
