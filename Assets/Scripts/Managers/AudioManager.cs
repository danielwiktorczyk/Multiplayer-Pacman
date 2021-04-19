using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip spookedSound;
    [SerializeField] private AudioClip pelletSound;
    [SerializeField] private AudioClip speedPelletSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySpookedSound()
    {
        audioSource.PlayOneShot(this.spookedSound);
    }

    public void PlayPelletSound()
    {
        audioSource.PlayOneShot(this.pelletSound);
    }

    public void PlaySpeedPelletSound()
    {
        audioSource.PlayOneShot(this.speedPelletSound);
    }
}
