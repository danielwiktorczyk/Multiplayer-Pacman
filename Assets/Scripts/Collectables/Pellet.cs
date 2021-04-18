using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public bool SpeedBoost;

    private AudioManager audioManager;

    private void Awake()
    {
        this.audioManager = FindObjectOfType<AudioManager>();
    }

    internal void OnPickedUp()
    {
        this.gameObject.SetActive(false);
        this.audioManager.PlayPelletSound();
    }
}
