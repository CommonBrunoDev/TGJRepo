using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private AudioClip openSFX;
    [SerializeField] private AudioClip closeSFX;
    [SerializeField] private Transform[] hinges = new Transform[2];
    public bool open = false;
    private float slerpTimer = 1;
    public void Open()
    {
        GetComponent<BoxCollider>().isTrigger =  true;
        GetComponent<AudioSource>().clip = openSFX;
        GetComponent<AudioSource>().Play();
        slerpTimer = 0;
    }

    public void Close()
    {
        GetComponent<BoxCollider>().isTrigger =  false;
        GetComponent<AudioSource>().clip = closeSFX;
        GetComponent<AudioSource>().Play();
        slerpTimer = 0;
    }

    private void Update()
    {
        if (slerpTimer < 1)
        {
            slerpTimer += Time.deltaTime / 4;
            if (slerpTimer > 1f) {slerpTimer = 1;}
        }
        

        hinges[0].rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0f, open ? 90f : 0f, 0f),
            slerpTimer);
        
        hinges[1].rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0f, open ? -90f : 0f, 0f),
            slerpTimer);
    }
}
