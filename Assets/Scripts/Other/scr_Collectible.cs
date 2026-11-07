using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string cName;
    private SphereCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {Debug.Log("Player Collided");}
        else
        {Debug.Log("Collision");}
        
    }
}
