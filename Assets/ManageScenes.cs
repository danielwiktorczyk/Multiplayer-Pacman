using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageScenes : MonoBehaviourPunCallbacks
{
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
            Debug.LogError("Photonetwork : Trying to Load a level but we are not the master Client");

        Debug.Log($"PhotonNetwork : Loading Level : {PhotonNetwork.CurrentRoom.PlayerCount}");
        PhotonNetwork.LoadLevel("RoomFor" + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log($"OnPlayerEnteredRoom() : {other.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"OnPlayerEnteredRoom() : IsMasterClient : {PhotonNetwork.IsMasterClient}");

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"OnPlayerLeftRoom() : {otherPlayer.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"OnPlayerLeftRoom() : IsMasterClient : {PhotonNetwork.IsMasterClient}");

            LoadArena();
        }
    }


}
