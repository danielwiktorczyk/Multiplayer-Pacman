using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public bool SpeedBoost;

    internal void OnPickedUp()
    {
        this.gameObject.SetActive(false);
    }
}
