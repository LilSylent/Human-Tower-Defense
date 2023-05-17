using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ToronyGomb : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    #region Változók
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI arText;
    private static Epiteskezelo kezelo;

    private RectTransform rT;
    private Vector3 eredetiPozicio;

    [SerializeField] private ToronyBlueprint torony;
    #endregion


    private void Awake()
    {
        //Megszerezzük annak a komponenseit, amin van a script.
        rT = GetComponent<RectTransform>(); //RectTransfrom

        //Letároljuk, hogy eredetileg hol van az amin a script van.
        eredetiPozicio = new Vector3(rT.anchoredPosition.x, rT.anchoredPosition.y);

        //Beállítjuk az építéskezelőt.
        kezelo = Epiteskezelo.Kezelo;

        arText.text = $"${torony.Ar}";
    }

    //Fogás elején fut egyszer
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //Átkonvertáljuk az egérpozíciót a Rect Transform pozíciójára
        Vector2 egerPozicio;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rT, eventData.position, eventData.pressEventCamera, out egerPozicio);

        //Beállítjuk a Rect Transform közepét az egérre
        rT.anchoredPosition = egerPozicio;
        //Elrejtjük az egeret
        Cursor.visible = false;

        //Elrejtük a fejlesztés UI-t
        kezelo.ToronyKijelolesTorlese();

        //Ha van Torony script a Prefab-en
        if (torony.Prefab.GetComponent<Torony>() != null)
        {
            //Letároljuk a hatótávot átmenetileg
            float hatotav = torony.Prefab.GetComponent<Torony>().Hatotav;
            //2x akkorára növeljük a RangeSprite-ot, amekkora a hatótáv
            kezelo.RangeSprite.transform.localScale = new Vector2(hatotav * 2, hatotav * 2);
        }
    }

    //Ameddig mozgatjuk az egeret fut végig
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rT.anchoredPosition += eventData.delta / canvas.scaleFactor;

        //Átkonvertáljuk a UI poziciót világ pozicióra.
        Vector2 pozicio = Camera.main.ScreenToWorldPoint(new Vector2(transform.position.x, transform.position.y));

        //Ha van Torony script a Prefab-en
        if (torony.Prefab.GetComponent<Torony>() != null)
        {
            //Bekapcsoljuk a RangeSprite-ot
            kezelo.RangeSprite.enabled = true;
            //A RangeSprite mindig az egér pozíciója lesz
            kezelo.RangeSprite.transform.position = pozicio;
        }

        //Ha jó helyre akarjuk lerakni a tornyot és nincs egymáson a két torony, plusz van elég pénzünk, akkor...
        if (Epiteskezelo.JoHegyenVan(pozicio) && !Epiteskezelo.EgymasonVan(pozicio) && torony.Ar <= JatekosStatisztika.JelenlegiPenz)
        {
            //RangeSprite-ot feketére állítjuk 30%-os áttetszőségre
            kezelo.RangeSprite.color = new Color(0f, 0f, 0f, 0.3f);
        }
        else
        {
            //RangeSprite-ot pirosra állítjuk 30%-os áttetszőségre
            kezelo.RangeSprite.color = new Color(255f, 0f, 0f, 0.3f);
        }
    }

    //Elengedéskor fut le egyszer
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //A RangeSprite-ot feketére állítjuk
        kezelo.RangeSprite.color = new Color(0f, 0f, 0f, 0.3f);

        //Kikapcsoljuk a RangeSprite-ot
        kezelo.RangeSprite.enabled = false;

        //Átkonvertáljuk a UI poziciót világ pozicióra.
        Vector3 pozicio = new Vector3(transform.position.x, transform.position.y);
        pozicio = Camera.main.ScreenToWorldPoint(pozicio);

        //Lerakjuk a turretet oda ahova kell.
        kezelo.Lerak(pozicio, torony);

        //Visszarögzítjük a turretet az eredeti helyére
        rT.anchoredPosition = eredetiPozicio;

        //Láthatóvá tesszük az egeret
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        rT.anchoredPosition = eredetiPozicio;
    }
}
