using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JatekosStatisztika : MonoBehaviour
{
    [SerializeField] private static Nehezsegek nehezseg;

    //Pénz
    [SerializeField] private int kezdoPenz = 850;
    private static int penz;

    //Élet
    [SerializeField] private int kezdoElet = 100;
    private static int elet;

    //Kör
    private static int kor;

    //Property-k
    public static int JelenlegiPenz { get => penz; }
    public static int JelenlegiElet { get => elet; }
    public static int JelenlegiKor { get => kor; }
    public static Nehezsegek JelenlegiNehezseg { get => nehezseg; }

    private void Start()
    {
        //Beállítjuk a nehézséget a kiválasztottra
        nehezseg = Nehesegvezerlo.Nehezseg;

        switch (nehezseg)
        {
            case Nehezsegek.Könnyű:
                KorSpawner.MaxKor = 5;
                break;
            case Nehezsegek.Közepes:
                kezdoElet /= 2;
                kezdoPenz = 1100;
                KorSpawner.MaxKor = 10;
                break;
            case Nehezsegek.Nehéz:
                kezdoElet /= 4;
                kezdoPenz = 1500;
                KorSpawner.MaxKor = 15;
                break;
        }

        penz = kezdoPenz;
        elet = kezdoElet;
        kor = 0;
    }

    public static void PenzLevonas(int mennyit)
    {
        penz -= mennyit;
    }
    public static void PenzHozaadas(int mennyit)
    {
        penz += mennyit;
    }

    public static void EletLevonas(int mennyit)
    {
        elet -= mennyit;

        //Ezzel küszöbölöm ki, hogy ne mehessünk minuszba
        if (elet < 0)
        {
            elet = 0;
        }
    }

    public static void KorNoveles()
    {
        kor++;
    }
    
    private void Update()
    {
        //P gomb lenyomásával pénzt adunk magunknak
        /*if (Input.GetKeyDown("p"))
        {
            penz += 100000;
        }*/
    }
}
