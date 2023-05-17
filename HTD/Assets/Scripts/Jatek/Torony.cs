using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Torony : MonoBehaviour
{
    #region Változók
    /// <summary>Ez a építéskezelő scriptünk</summary>
    private Epiteskezelo kezelo;

    /// <summary>Ez a célpont, amit a torony követ</summary>
    private Transform celpont;

    [Header("Torony beállítások")]
    [SerializeField] private float hatotav = 2f;
    [SerializeField] private float tuzgyorsasag = 1f;
    [SerializeField] private float forgasiSebesseg = 10f;

    [Header("Unity-s beállítások")]
    [SerializeField] private string ellensegJeloles = "Ellenseg";
    [SerializeField] private Transform amitForgatunk;

    //[SerializeField] private string loveshang;

    [SerializeField] private GameObject golyoPrefab;
    [SerializeField] private Transform lovesPont;

    private float tuzVisszaszamlalo = 0f;

    private ToronyBlueprint toronyBlueprint;
    private bool fejlesztve = false;
    #endregion

    #region Property-k
    public ToronyBlueprint ToronyBlueprint { get => toronyBlueprint; set => toronyBlueprint = value; }
    public bool Fejlesztve { get => fejlesztve; set => fejlesztve = value; }
    public Epiteskezelo Kezelo { get => kezelo; }
    public float Hatotav { get => hatotav; set => hatotav = value; }
    #endregion

    private void Start()
    {
        //Fél másodpercenként meghívjuk a Célpont frissítést
        InvokeRepeating("CelpontFrissitese", 0f, 0.5f);

        //Beállítjuk kezelőnek az építéskezelőt.
        kezelo = Epiteskezelo.Kezelo;
    }

    private void Update()
    {
        //Ha nincs célpont, akkor...
        if (celpont == null)
        {
            //Ne fusson le a kód további része
            return;
        }

        //Célpontra rögzítjük a Tornyunkat, hogy ne csak egyhelybe álljon.
        Vector3 irany = celpont.position - transform.position;
        Quaternion forgasiIrany = Quaternion.LookRotation(irany, Vector3.forward);
        Vector3 forgas = Quaternion.Lerp(amitForgatunk.rotation, forgasiIrany, Time.deltaTime * forgasiSebesseg).eulerAngles;
        amitForgatunk.rotation = Quaternion.Euler(0f, 0f, forgas.z);

        //Csak akkor tudunk lőni, ha a visszaszámláló <= 0.
        if (tuzVisszaszamlalo <= 0f)
        {
            Tuzeles();
            //A tűzgyorsaság befolyásolja a visszaszámlálót.
            tuzVisszaszamlalo = 1f / tuzgyorsasag;
        }

        //A két frame között eltelt időt kivonjuk a visszaszámlálásból.
        tuzVisszaszamlalo -= Time.deltaTime;
    }

    private void CelpontFrissitese() {
        //Letároljuk az ellenségeket egy tömbbe
        GameObject[] ellensegek = GameObject.FindGameObjectsWithTag(ellensegJeloles);
        //A legrövidebb táv alapból végtelen
        float legrovidebbTavolsag = Mathf.Infinity;
        //És a legközelebbi ellenség alapból üres
        GameObject legkozelebbiEllenseg = null;

        //Végigjárjuk az ellenségeket
        foreach (GameObject ellenseg in ellensegek)
        {
            //Kiszámoljuk, hogy milyen messze van az ellenség
            float tavolsagAzEllensegig = Vector3.Distance(transform.position, ellenseg.transform.position);

            //És ha a kiszámolt táv kisebb, mint a legrövidebb táv, akkor..
            if (tavolsagAzEllensegig < legrovidebbTavolsag)
            {
                //Letároljuk legrövidebb távnak
                legrovidebbTavolsag = tavolsagAzEllensegig;
                //És letároljuk azt az ellenséget
                legkozelebbiEllenseg = ellenseg;
            }
        }

        //Ha van legközelebbi ellenség és a legrövidebb táv hatótávon belül van, akkor...
        if (legkozelebbiEllenseg != null && legrovidebbTavolsag <= hatotav)
        {
            //Átadjuk az ellenséget célpontnak
            celpont = legkozelebbiEllenseg.transform;
        }
        else
        {
            //Ha viszont nincs, akkor nincs célpont se
            celpont = null;
        }
    }

    //Ez csinálja azt, hogy az editorba lássuk a turret hatótávját, amikor kiválasztjuk.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, hatotav);
    }

    private void Tuzeles() {
        //Debug.Log("Lövés");
        //FindObjectOfType<Hangkezelo>().Lejatszas(loveshang);

        //Megidézzük a golyót
        GameObject golyoGameObject = (GameObject)Instantiate(golyoPrefab, lovesPont.position, lovesPont.rotation);
        Golyo golyo = golyoGameObject.GetComponent<Golyo>();

        //Ha van golyónk, akkor
        if (golyo != null)
        {
            //Az megkeresi a célpontját.
            golyo.Kereses(celpont);
        }
    }

    private void OnMouseDown()
    {
        //Ellenörzi, hogy UI-elemen keresztül kattintottunk-e
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Ha igaz, akkor nem futtatjuk a kód további részét
            return;
        }

        kezelo.ToronyKijeloles(this);
    }
}
