using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hangkezelo : MonoBehaviour
{
    private static Hangkezelo kezelo;
    [SerializeField] 
    private Hang[] hangok;

    private void Awake()
    {
        //Ha nincs kezelőnk, akkor...
        if (kezelo == null)
        {
            //Ez lesz
            kezelo = this;
        }
        else
        {
            //Ha viszont már van, akkor az újat töröljük
            Destroy(gameObject);
            //És nem futtatjuk tovább a kódot
            return;
        }

        //Beállítjuk, hogy ne törlődjön a gameObject
        DontDestroyOnLoad(gameObject);

        //Végigmegyünk a hangokon és...
        foreach (var hang in hangok)
        {
            //Hozzáadjuk a gameObject-hez az AudioSource-t
            hang.Forras = gameObject.AddComponent<AudioSource>();
            //Az AudioClip-et
            hang.Forras.clip = hang.Klip;
            //Az AudioMixerGroup-ot
            hang.Forras.outputAudioMixerGroup = hang.Mixer;
            //A loopot
            hang.Forras.loop = hang.Loop;
            hang.Forras.playOnAwake = false;
        }
    }

    private void Start()
    {
        Lejatszas("Zene");
    }

    public void Lejatszas(string nev)
    {
        //Megkeressük azt a hangot, amit paraméterben megadtunk
        Hang hang = Array.Find(hangok, x => x.Nev == nev);

        //Ha nincs ilyen hang, akkor...
        if (hang == null)
        {
            //Jelezzük a Log-ban
            Debug.LogWarning($"Ez a hang: {hang.Nev} nem található");
            //És nem futtatjuk tovább
            return;
        }

        //Lejátszuk a hangot
        hang.Forras.Play();
    }
}
