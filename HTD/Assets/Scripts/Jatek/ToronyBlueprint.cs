using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToronyBlueprint
{
    #region Változók
    [SerializeField] private string nev;

    [SerializeField] private GameObject effekt;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int ar;
    [SerializeField] private Sprite ikon;

    [SerializeField] private GameObject fejlesztettPrefab;
    [SerializeField] private int fejlesztesiAr;
    [SerializeField] private Sprite fejlesztettIkon;
    #endregion

    /// <summary>Megadja, hogy mennyiért tudjuk eladni a tornyot</summary>
    public int eladasiAr(bool fejlesztve)
    {
        //Ha a torony fejlesztve van, akkor...
        if (fejlesztve)
        {
            //Visszaadjuk a torony árát + a fejlesztési árát osztva 1.5-el
            return Convert.ToInt32(ar + fejlesztesiAr / 1.5);
        }
        else
        {
            //Visszaadjuk a torony árát osztva 1.5-el
            return Convert.ToInt32(ar / 1.5);
        }
    }

    public string Nev { get => nev; set => nev = value; }
    public GameObject Effekt { get => effekt; set => effekt = value; }

    //Alap
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Ar { get => ar; set => ar = value; }
    public Sprite Ikon { get => ikon; set => ikon = value; }

    //Fejlesztett
    public GameObject FejlesztettPrefab { get => fejlesztettPrefab; set => fejlesztettPrefab = value; }
    public int FejlesztesiAr { get => fejlesztesiAr; set => fejlesztesiAr = value; }
    public Sprite FejlesztettIkon { get => fejlesztettIkon; set => fejlesztettIkon = value; }
}
