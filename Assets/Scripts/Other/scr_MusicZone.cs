using System;
using System.Collections.Generic;
using UnityEngine;

public class scr_MusicZone : MonoBehaviour
{
    [SerializeField] private List<Gate> gates;
    [SerializeField] private int[] code = new int[5];
    [SerializeField] public AudioClip music;
    [SerializeField] public AudioClip codeMusic;
    private bool playerIn = false;
    public bool active = false;

    private void Update()
    {
        if (playerIn && active)
        {
            if (InputManager.Instance.StartStopMusic)
            {
                scr_PlayerController.Instance.isPlayingMusic = !scr_PlayerController.Instance.isPlayingMusic;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;
            if (active)
            { foreach (Gate g in gates)   g.Close(); }

            GetComponent<AudioSource>().Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {playerIn = false;}
    }

    public void OpenGates()
    {
        { foreach (Gate g in gates) g.Open(); }
    }
    public void HintMusic()
    {
        GetComponent<AudioSource>().clip = codeMusic;
        GetComponent<AudioSource>().Play();
    }
}
