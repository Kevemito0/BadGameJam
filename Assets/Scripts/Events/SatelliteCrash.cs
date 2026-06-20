using System;
using UnityEngine;

public class SatelliteCrash : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip crashSound;
    private bool crashed;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (crashed) return;

        crashed = true;
        
        audioSource.PlayOneShot(crashSound);
    }
}
