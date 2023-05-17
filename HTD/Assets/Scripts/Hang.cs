using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Hang
{
    [SerializeField] private string nev;
    [SerializeField] private AudioClip klip;
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private bool loop;

    private AudioSource forras;

    public string Nev { get => nev; set => nev = value; }
    public AudioClip Klip { get => klip; set => klip = value; }
    public AudioMixerGroup Mixer { get => mixer; set => mixer = value; }
    public bool Loop { get => loop; set => loop = value; }
    public AudioSource Forras { get => forras; set => forras = value; }
}
