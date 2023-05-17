using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KorSpawner : MonoBehaviour
{
    private static int eletbenLevoEllenseg = 0;
    [SerializeField] private Kor[] korok;
    [SerializeField] private Transform kezdoPont;

    private int korIndex = 0;
    private static int maxKor = 0;

    private bool megallitva = true;
    [SerializeField] private Button megallitoGomb;

    [SerializeField] private float korokKoztiIdo = 2f;
    private float visszaszamlalo = 2f;

    private Jatekvezerlo jatekvezerlo;

    public static int EletbenLevoEllenseg { get => eletbenLevoEllenseg; set => eletbenLevoEllenseg = value; }
    public static int MaxKor { get => maxKor; set => maxKor = value; }

    private void Awake()
    {
        //Megszerezzük a játékvezérlőt
        jatekvezerlo = this.GetComponent<Jatekvezerlo>();
        //Életben lévő ellenségeket 0-ra állítjuk
        eletbenLevoEllenseg = 0;
    }

    private void Update()
    {
        //Ha van életben lévő ellenség, akkor...
        if (eletbenLevoEllenseg > 0)
        {
            //Nem futtatjuk tovább a scriptet.
            return;
        }

        //Ha a kör index és a körök hossza vagy a kör index és a max kör megegyezik, akkor...
        if (korIndex == korok.Length || korIndex == maxKor)
        {
            //Meghívjuk a Nyerés metódust
            jatekvezerlo.Nyeres();

            //És kikapcsoljuk ezt a scriptet
            this.enabled = false;
        }

        //Ha nincs megállítva a játék és a visszaszámlálónk <= 0, akkor...
        if (!megallitva && visszaszamlalo <= 0f)
        {
            //Kikapcsoljuk a megállító gombot
            megallitoGomb.gameObject.SetActive(false);

            StartCoroutine(KorNoveles());

            //A visszaszámlálónk a Coroutine után visszaáll alaphelyzetbe
            visszaszamlalo = korokKoztiIdo;

            //Visszatérünk
            return;
        }

        visszaszamlalo -= Time.deltaTime;
    }

    private IEnumerator KorNoveles() {
        //Debug.Log("ITT JÖN A HORDA");

        //Növeljük a Játékos Statisztikában a kört
        JatekosStatisztika.KorNoveles();

        //Jelenlegi kört átadjuk
        Kor kor = korok[korIndex];

        //Az életben lévő ellenséget egyből a kör maximális ellenségszámával tesszük egyenlővé
        eletbenLevoEllenseg = kor.Mennyiseg;

        //Végigmegyünk az ellenség mennyiségén
        for (int i = 0; i < kor.Mennyiseg; i++)
        {
            //És lerakjuk
            EllensegLerakas(kor.Ellenseg);

            //Várunk annyi időt, amennyit előre megszabtunk
            yield return new WaitForSeconds(kor.Idokoz);
        }

        //Növeljük a kör index-et
        korIndex++;
    }

    private void EllensegLerakas(GameObject ellenseg) {
        //Lerakjuk az ellenséget
        Instantiate(ellenseg, kezdoPont.position, kezdoPont.rotation);
    }

    public void StartStop()
    {
        //Ha nincs megállítva -> megállítjuk, és fordítva
        megallitva = !megallitva;
    }
}
