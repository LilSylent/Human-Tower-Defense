using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JatekNyeres : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private TextMeshProUGUI korText;

    //Akkor hívjuk meg amikor bekapcsoljuk a UI-t
    private void OnEnable()
    {
        korText.text = $"{JatekosStatisztika.JelenlegiKor} kör";
    }

    public void Menu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
