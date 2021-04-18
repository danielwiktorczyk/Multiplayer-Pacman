using Photon.Pun;
using UnityEngine;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
	private const string roomName = "TheOneRoomToRuleThemAllAndInTheDarkenssBindThem";

	private const string player1 = "player1";
	private const string player2 = "player2";

	[SerializeField] private Transform player1Transform;
	[SerializeField] private Transform player2Transform;

	void Start()
	{
		Debug.Log("==Connecting using settings==");
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("==Connected to Master==");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("==Joined Room!==");
		if (PhotonNetwork.IsMasterClient)
			PhotonNetwork.Instantiate(player1, player1Transform.position, Quaternion.identity, 0);
		else
			PhotonNetwork.Instantiate(player2, player2Transform.position, Quaternion.identity, 0);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("==Join Room failed. Attempting to Create a room==");
		PhotonNetwork.CreateRoom(roomName);
	}
}
