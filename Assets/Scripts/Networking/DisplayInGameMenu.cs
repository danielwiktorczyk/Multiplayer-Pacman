using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayInGameMenu : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("InGameMenu", LoadSceneMode.Additive);
    }

}
