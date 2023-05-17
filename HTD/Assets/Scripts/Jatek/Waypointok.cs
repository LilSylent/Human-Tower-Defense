using UnityEngine;

public class Waypointok : MonoBehaviour
{
    private static Transform[] pontok;

    public static Transform[] Pontok { get => pontok; set => pontok = value; }

    private void Awake() {
        //A tömb akkora lesz, ahány gyereke van.
        pontok = new Transform[transform.childCount];

        //A tömbbe belerakjuk a gyerekeit.
        for (int i = 0; i < Pontok.Length; i++) {
            pontok[i] = transform.GetChild(i);
        }
    }
}
