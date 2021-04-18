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
    public GameManager gameManager;


    private bool isRemovedFromPlay;

    private static int TotalRemainingPellets;

    private void Awake()
    {
        this.audioManager = FindObjectOfType<AudioManager>();
        this.photonView = GetComponent<PhotonView>();
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        TotalRemainingPellets += 1;
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

        RemoveFromPlay();

        TotalRemainingPellets -= 1;
        if (TotalRemainingPellets <= 0)
            this.gameManager.EndGame();
    }

    private void RemoveFromPlay()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.isRemovedFromPlay = true;
    }
}
