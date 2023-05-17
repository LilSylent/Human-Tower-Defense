using UnityEngine;

public class Golyo : MonoBehaviour
{
    private Transform celpont;

    [SerializeField] private float gyorsasag = 100f;
    [SerializeField] private int sebzes = 50;
    [SerializeField] private GameObject effekt;
    
    public void Kereses(Transform _celpont) {
        celpont = _celpont;
    }

    private void Update()
    {
        //Ha nincs célpont, töröljük a golyót és a kód további részét nem futtatjuk.
        if (celpont == null)
        {
            Destroy(gameObject);
            return;
        }

        //Kiszámoljuk az Golyó és a Célpont közti irányt.
        Vector3 irany = celpont.position - transform.position;

        float tavolsag = gyorsasag * Time.deltaTime;

        //Ha az irány hossza <= mint a távolság, akkor eltaláljuk a célpontot és nem futtatjuk a kód további részét.
        if (irany.magnitude <= tavolsag)
        {
            CelpontEltalalasa();
            return;
        }

        //A Golyót a célpont felé irányítjuk.
        transform.Translate(irany.normalized * tavolsag, Space.World);
    }

    private void CelpontEltalalasa() {
        //Lerakjuk az enemy helyére az effektet
        Instantiate(effekt, transform.position, transform.rotation);
        

        //Debug.Log("Találat");
        Sebzes(celpont);

        //Kitöröljük a golyót
        Destroy(gameObject);
    }

    private void Sebzes(Transform ellenseg)
    {
        Ellenseg e = ellenseg.GetComponent<Ellenseg>();

        //Ha van ellenség komponensünk...
        if (e != null)
        {
            //akkor levonunk x mennyiségű értéket
            e.Sebzes(sebzes);
        }
    }
}
