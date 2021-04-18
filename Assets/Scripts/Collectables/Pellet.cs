using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public bool SpeedBoost;

    private AudioManager audioManager;
    public PhotonView photonView;

    private bool isRemovedFromPlay;

    private void Awake()
    {
        this.audioManager = FindObjectOfType<AudioManager>();
        this.photonView = GetComponent<PhotonView>();
    }

    public bool IsRemovedFromPlay()
    {
        if (!this.isRemovedFromPlay)
            return false;

        RemoveFromPlay(); // ensure it is removed

        return true;
    }

    internal void OnPickedUp()
    {
        if (this.isRemovedFromPlay)
        {
            Debug.LogWarning("I'm supposed to be removed from play!", gameObject);
            return;
        }

        this.gameObject.SetActive(false);
        this.audioManager.PlayPelletSound();

        RemoveFromPlay();
    }

    private void RemoveFromPlay()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.isRemovedFromPlay = true;
    }
}
