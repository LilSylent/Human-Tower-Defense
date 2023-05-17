using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SzunetMenu : MonoBehaviour
{
    [SerializeField] private GameObject szunetMenu;
    [SerializeField] private GameObject beallitasokMenu;

    private void Update()
    {
        //Ha lenyomjuk az Escape gombot és nincs bekapcsolva a beállítások menü lefut a KiBe()
        if (Input.GetKeyDown(KeyCode.Escape) && !beallitasokMenu.activeSelf && !Jatekvezerlo.VegetErt)
        {
            KiBe();
        }
    }

    public void KiBe()
    {
        //Ha nincs bekapcsolva bekapcsoljuk (megnyitjuk), ha pedig bevan, akkor kikapcsoljuk
        szunetMenu.SetActive(!szunetMenu.activeSelf);

        //Ha aktiválva van...
        if (szunetMenu.activeSelf)
        {
            //...megállítjuk az időt.
            Time.timeScale = 0f;
        }
        else
        {
            //Különben visszarakjuk normális időre.
            Time.timeScale = 1f;
        }
    }

    public void Menu()
    {
        Jatekvezerlo.Menu();
    }

    public void Ujrakezdes()
    {
        Jatekvezerlo.Ujrakezdes();
    }
}
