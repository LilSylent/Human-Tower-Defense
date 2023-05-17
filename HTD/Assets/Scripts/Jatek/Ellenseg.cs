using UnityEngine;
using UnityEngine.UI;

public class Ellenseg : MonoBehaviour
{
    [SerializeField] private float gyorsasag = 5f;
    [SerializeField] private int elet = 100;
    [SerializeField] private int penzErtek = 30;
    [SerializeField] private int sebzesErtek = 5;

    [SerializeField] private Slider eletcsik;

    private bool halott;

    private Transform celpont;
    private int waypointIndex;

    private void Start()
    {
        //Játék indításakor a célpontnak a legelső waypoint-ot állítjuk be.
        celpont = Waypointok.Pontok[0];

        //Beállítjuk az ellenség életcsíkját
        eletcsik.maxValue = elet;
        eletcsik.value = elet;
    }

    private void Update()
    {
        //Kiszámoljuk az Enemy és a Célpont közti irányt.
        Vector3 irany = celpont.position - transform.position;

        //Az Enemyt a waypoint felé irányítjuk.
        transform.Translate(irany.normalized * gyorsasag * Time.deltaTime, Space.World);

        //Megfelelő irányba forgatjuk az enemyt, hogy ne mindig ugyanarra nézzen.
        float szog = Mathf.Atan2(irany.y, irany.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(szog, Vector3.forward);

        //Ha az Enemy és a Célpont közelebb van mint 0.1 akkor lefut a KovetkezoPont() metódus.
        if (Vector2.Distance(transform.position, celpont.position) <= 0.1f) 
        {
            KovetkezoWaypoint();
        }
    }

    public void Sebzes(int mennyit)
    {
        //Levonunk az ellenség jelenlegi életéből x mennyiséget.
        elet -= mennyit;
        //Az új életre beállítjuk az életcsík értékét.
        eletcsik.value = elet;

        //Ha a jelenlegi élet <= 0 és nem halott, akkor...
        if (elet <= 0 && !halott)
        {
            //FindObjectOfType<Hangkezelo>().Lejatszas("Halal");
            //Beállítjuk a halott értéket igazra, mivel volt olyan eset, hogy többször tudott meghalni
            halott = true;

            //Megsemmisítjük az ellenséget
            Destroy(gameObject);

            //Adunk a játékosnak meghatározott pénzmennyiséget.
            JatekosStatisztika.PenzHozaadas(penzErtek);

            //Levonunk az életben lévő ellenségek közül egyet.
            KorSpawner.EletbenLevoEllenseg--;
        }
    }

    private void KovetkezoWaypoint() {
        //Ha az Index nagyobb egyenlő mint a Waypoint tömb hossza-1, akkor elértük az utolsó waypointot, magyarul bejutott az ellenség
        if (waypointIndex >= Waypointok.Pontok.Length - 1)
        {
            //Megsemmisétjük az ellenséget
            Destroy(gameObject);

            //Levonunk a Játékosunk életéből x mennyiséget.
            JatekosStatisztika.EletLevonas(sebzesErtek);

            //Levonunk az életben lévő ellenségek közül egyet.
            KorSpawner.EletbenLevoEllenseg--;

            //Visszatérünk
            return;
        }
        
        //Növeljük a waypoint index-et...
        waypointIndex++;

        //És azt átadjuk a célpontnak.
        celpont = Waypointok.Pontok[waypointIndex];
    }
}
