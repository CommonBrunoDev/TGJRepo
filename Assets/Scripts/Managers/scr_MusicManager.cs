using System;
using UnityEngine;

public class scr_MusicManager : Singleton<scr_MusicManager>
{
    private AudioSource audio;
    private float dampenTimer = 2;

    public void PlayMusic(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }

    public void DampenMusic()
    {
        dampenTimer = 2;
    }

    public void Update()
    {
        if (dampenTimer > 0)
        {
            dampenTimer -= Time.deltaTime;
            audio.volume = dampenTimer / 2;
            if (dampenTimer <= 0) audio.Stop();
        }
    }
}
