using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class Adatbazisteszt
{
    [Test]
    public void SikeresBelepes_Teszt()
    {
        Assert.IsTrue(Felhasznalokezelo.Db.Belepes("Teszt", "Teszt"));
    }

    [Test]
    public void SikertelenBelepes_Teszt()
    {
        Assert.IsFalse(Felhasznalokezelo.Db.Belepes("sjdkfskjxckvksjfksjvxcvkjdsfk", "123"));
    }

    [Test]
    public void SikertelenRegisztracio_Teszt()
    {
        Assert.IsFalse(Felhasznalokezelo.Db.Regisztralas("Teszt", "123"));
    }

    [Test]
    public void SHAKonvertalas_Teszt()
    {
        string vart = "b3ac6b78a63a0387f55257d764fa8787118fac54963c8746f0e99d2234fc70a2";

        string kapott = Felhasznalokezelo.Db.SHA256Szamolas("ezegytesztjelszó");

        Assert.AreEqual(vart, kapott);
    }
}
