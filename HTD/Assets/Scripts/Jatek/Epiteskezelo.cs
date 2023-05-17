using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Epiteskezelo : MonoBehaviour
{
    #region Változók
    /// <summary>Ez maga a pályának a Tilemap-je</summary>
    [SerializeField] private Tilemap groundTilemap;
    /// <summary>Ez maga a pályán lévő környezeti elemek Tilemap-je</summary>
    //[SerializeField] private Tilemap environmentTilemap;
    /// <summary>Ez az a tile, amire lehet pakolni</summary>
    [SerializeField] private Tile[] foldTile;

    /// <summary>Ebben van tárolva a Fejlesztés/Eladás UI-unk</summary>
    [SerializeField] private ToronyUI toronyUI;

    /// <summary>Ez az építéskezelő scriptünk</summary>
    private static Epiteskezelo kezelo;

    /// <summary>Ebben van tárolva a kijelölt torony</summary>
    private Torony kijeloltTorony;
    /// <summary>Ez az a kör, ami a tornyok körül megjelenik</summary>
    private SpriteRenderer rangeSprite;
    #endregion

    #region Property-k
    /// <summary>Ez a építéskezelő scriptünk</summary>
    public static Epiteskezelo Kezelo { get => kezelo; }
    /// <summary>Ez az a kör, ami a tornyok körül megjelenik</summary>
    public SpriteRenderer RangeSprite { get => rangeSprite; set => rangeSprite = value; }
    /// <summary>Ez maga a pályának a Tilemap-je</summary>
    public Tilemap GroundTilemap { get => groundTilemap; }
    /// <summary>Ez az a tile, amire lehet pakolni</summary>
    public Tile[] FoldTile { get => foldTile; }
    #endregion

    private void Awake()
    {
        //Ha a script betöltésekor már van a kezelőbe bármi, akkor...
        if (kezelo != null)
        {
            //Hibát dobunk
            Debug.LogError("TÖBB MINT EGY ÉPÍTÉSKEZELŐNK VAN!");

            //És nem hagyjuk futni a kód további részét
            return;
        }

        //Amúgy meg beállítjuk ezt a scriptet kezelőnek
        kezelo = this;

        //Megszerezzük a rangeSprite-ot
        rangeSprite = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>Lerak egy általunk átadott tornyot, egy általunk átadott helyre</summary>
    public void Lerak(Vector2 hova, ToronyBlueprint torony)
    {
        //Ha jó helyre akarjuk lerakni a tornyot és nincs egymáson a két torony, plusz van elég pénzünk, akkor...
        if (JoHegyenVan(hova) && !EgymasonVan(hova) &&
            torony.Ar <= JatekosStatisztika.JelenlegiPenz)
        {
            //Lementjük a tornyot egy segédváltozóba, illetve le is rakjuk a pályára
            GameObject ujTorony = Instantiate(torony.Prefab, new Vector3(hova.x, hova.y, -0.1f), Quaternion.identity);

            //Megkeressük a torony Torony script-jét, és azt letároljuk átmenetileg
            Torony _ujTorony = ujTorony.GetComponent<Torony>();
            //A tornyon beállítjuk a blueprint-jét
            _ujTorony.ToronyBlueprint = torony;

            //Levonjuk a játékostól az árat
            JatekosStatisztika.PenzLevonas(torony.Ar);

            //Lerakjuk az effektet
            Instantiate(torony.Effekt, ujTorony.transform.position, ujTorony.transform.rotation);

            //Lejátszuk a lerakás hangot
            FindObjectOfType<Hangkezelo>().Lejatszas("Lerakas");

            //Kiírjuk DebugLog-ra
            //Debug.Log($"Maradék pénz: {JatekosStatisztika.JelenlegiPenz}");
        }
    }

    /// <summary>Fejleszti azt a tornyot, amire meghívjuk</summary>
    public void ToronyFejlesztes()
    {
        //Ha a tornyunk meg nincs fejlesztve és a fejlesztés összege <= mint a jelenlegi pénze a játékosnak, akkor...
        if (!kijeloltTorony.Fejlesztve && kijeloltTorony.ToronyBlueprint.FejlesztesiAr <= JatekosStatisztika.JelenlegiPenz)
        {
            //Lementjük a fejlesztett tornyot egy segédváltozóba, illetve le is rakjuk a pályára
            GameObject ujTorony = Instantiate(kijeloltTorony.ToronyBlueprint.FejlesztettPrefab, new Vector3(kijeloltTorony.transform.position.x, kijeloltTorony.transform.position.y, kijeloltTorony.transform.position.z), transform.rotation);

            //Levonjuk a fejlesztés összegét a játékostól
            JatekosStatisztika.PenzLevonas(kijeloltTorony.ToronyBlueprint.FejlesztesiAr);

            //Megkeressük az új torony Torony script-jét, és azt letároljuk átmenetileg
            Torony _ujTorony = ujTorony.GetComponent<Torony>();
            //Az új tornyon beállítjuk, hogy már fejlesztve van
            _ujTorony.Fejlesztve = true;
            //Megszerezzük a régi torony blueprintjét
            _ujTorony.ToronyBlueprint = kijeloltTorony.ToronyBlueprint;

            //Lerakjuk az effektet
            Instantiate(kijeloltTorony.ToronyBlueprint.Effekt, ujTorony.transform.position, ujTorony.transform.rotation);

            //Lejátszuk a lerakás hangot
            FindObjectOfType<Hangkezelo>().Lejatszas("Lerakas");

            //Kitöröljük a régi tornyot
            Destroy(kijeloltTorony.gameObject);

            //A kijelölt tornyot null-ra állítjuk, mivel ekkor épp nincs semmi tornyunk
            kijeloltTorony = null;

            //Kijelöljük az új tornyot, hogy azt a UI-t mutassa
            ToronyKijeloles(_ujTorony);
        }
    }

    /// <summary>Eladja azt a tornyot, amire meghívjuk</summary>
    public void ToronyEladas()
    {
        //Megadjuk a toronyért járó pénzt a játékosnak
        JatekosStatisztika.PenzHozaadas(kijeloltTorony.ToronyBlueprint.eladasiAr(kijeloltTorony.Fejlesztve));

        //Töröljük a tornyot
        Destroy(kijeloltTorony.gameObject);

        ToronyKijelolesTorlese();
    }

    /// <summary>Kijelöli azt a tornyot, amit átadunk neki paraméterbe</summary>
    public void ToronyKijeloles(Torony t)
    {
        //Ha a kijelölt torony egyezik az új toronnyal, akkor...
        if (kijeloltTorony == t)
        {
            //Töröljük a kijelölést
            ToronyKijelolesTorlese();
            //Nem futtatjuk tovább
            return;
        }

        //Kijelölt torony egyenlő az új toronnyal
        kijeloltTorony = t;

        //A RangeSprite-ot pedig beállítjuk akkorára és oda, ahol a tornyunk van
        RangeSprite.transform.position = new Vector2(kijeloltTorony.transform.position.x, kijeloltTorony.transform.position.y);
        RangeSprite.transform.localScale = new Vector2(kijeloltTorony.Hatotav * 2, kijeloltTorony.Hatotav * 2);
        RangeSprite.enabled = true;

        //Megjelenítjük a UI-t
        toronyUI.ToronyBeallitas(t);
    }

    /// <summary>Törli a kijelölést</summary>
    public void ToronyKijelolesTorlese()
    {
        RangeSprite.enabled = false;

        //A kijelölt tornyot üresre állítjuk
        kijeloltTorony = null;

        //Elrejtjük a UI-t
        toronyUI.Elrejtes();
    }

    #region Ellenőrzések
    /// <summary>Megmondja, hogy egy másik toronyra akarjuk-e rárakni a tornyunkat</summary>
    public static bool EgymasonVan(Vector2 pozicio)
    {
        //Ez tárolja a Torony tag-el ellátott objektumokat
        GameObject[] objektumokTaggel = GameObject.FindGameObjectsWithTag("Torony");
        bool egymason = false;

        //Végigmegyünk a tömbön
        foreach (GameObject obj in objektumokTaggel)
        {
            //Ha az objektumon lévő BoxCollider és az egér pozíciója között van átfedés, akkor...
            if (obj.gameObject.GetComponent<BoxCollider2D>().OverlapPoint(new Vector3(pozicio.x, pozicio.y, -0.1f)))
            {
                //Egymáson van a két dolog
                egymason = true;
            }
        }

        //Visszaadjuk az igaz/hamis értéket
        return egymason;
    }

    /// <summary>Megmondja, hogy jó helyre akarjuk-e lerakni a tornyot</summary>
    public static bool JoHegyenVan(Vector2 pozicio)
    {
        //Átkonvertáljuk a Világ pozíciót (World Point) cella pozícióra
        Vector3Int gridPozicio = kezelo.GroundTilemap.WorldToCell(pozicio);
        bool jo = false;

        //Végigmegyünk a FoldTile tömbön
        for (int i = 0; i < kezelo.FoldTile.Length; i++)
        {
            //Ha lerakhatjuk a Tile-ra, akkor...
            if (kezelo.groundTilemap.GetTile(gridPozicio) == kezelo.FoldTile[i])
            {
                //Igazra állítjuk
                jo = true;
            }
        }

        //Visszaadjuk az igaz/hamis értéket
        return jo;
    }
    #endregion
}
