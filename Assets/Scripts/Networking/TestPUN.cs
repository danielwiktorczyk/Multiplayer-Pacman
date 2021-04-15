using Photon.Pun;
using System;
using UnityEngine;

public class TestPUN : MonoBehaviourPunCallbacks
{

    void Start()
    {
        Connect();
    }

    public void Connect()
    {
        Debug.Log("==Connecting to name server...==");
        PhotonNetwork.ConnectUsingSettings();
    }

    private void JoinRandomRoom()
    {
        Debug.Log("==Attempting to join a room...==");
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        Debug.Log("==Attemting to create a room...==");
        PhotonNetwork.CreateRoom("DunkeyDunks");
    }

    public override void OnConnected()
    {
        Debug.Log("==Connected TO MAIN SERVER==");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("==CONNECTED TO MASTER==");
        JoinRandomRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("==Successfully CREATED a room==");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("==Successfully JOINED a room==");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"return code: {returnCode}, message: {message}");
        Debug.Log("==Could not join a random room. Attempting to create a room instead==");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("==Failed to create a room==");
    }


}
