using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuUIController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text playerNameLabel;

    private void Awake()
    {
        this.playerNameLabel.text = PhotonNetwork.NickName;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public void ChangeScene()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
