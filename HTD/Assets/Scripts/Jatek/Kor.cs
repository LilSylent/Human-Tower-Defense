using UnityEngine;

[System.Serializable]
public class Kor
{
    [SerializeField] private GameObject ellenseg;
    [SerializeField] private int mennyiseg;
    [SerializeField] private float idokoz;

    public GameObject Ellenseg { get => ellenseg; set => ellenseg = value; }
    public int Mennyiseg { get => mennyiseg; set => mennyiseg = value; }
    public float Idokoz { get => idokoz; set => idokoz = value; }
}
