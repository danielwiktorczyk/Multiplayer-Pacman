using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
	private const string roomName = "TheOneRoomToRuleThemAllAndInTheDarkenssBindThem";

	private const string player1 = "player1";
	private const string player2 = "player2";

	[SerializeField] private Transform player1Transform;
	[SerializeField] private Transform player2Transform;

	private Text gameStatusUI;
	public bool IsGameStarted;
	private bool IsGameStarting;
	private float gameStartCooldown = 3f;
	private float gameReadyCooldown = 1f;
	private bool isGameReadyCooldownComplete;

	private PhotonView photonView;

	private void Awake()
	{
		this.gameStatusUI = GameObject.FindGameObjectWithTag("gamestatusui").GetComponent<Text>();
		this.photonView = GetComponent<PhotonView>();
	}

	void Start()
	{
		Debug.Log("==Connecting using settings==");
		PhotonNetwork.ConnectUsingSettings();
	}

	private void Update()
	{
		if (this.IsGameStarting)
			GameStartCountdown();

		if (IsGameStarted && !this.isGameReadyCooldownComplete)
			GameReadyGoMessage();
	}

	private void GameReadyGoMessage()
	{
		if (this.gameReadyCooldown < 0)
		{
			this.gameStatusUI.text = ""; // remove the message
			this.isGameReadyCooldownComplete = true;
		}
		else
		{
			this.gameReadyCooldown -= Time.deltaTime;
		}
	}

	private void GameStartCountdown()
	{
		if (this.gameStartCooldown < 0)
		{
			this.IsGameStarted = true;
			this.IsGameStarting = false;
			this.gameStatusUI.text = $"Go!";
		}
		else
		{
			this.gameStartCooldown -= Time.deltaTime;
			this.gameStatusUI.text = $"{Mathf.CeilToInt(this.gameStartCooldown)}";
		}
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
		{
			Debug.Log("==Deploying player1, and awaiting player2 for game start==");
			PhotonNetwork.Instantiate(player1, player1Transform.position, Quaternion.identity, 0);
			this.gameStatusUI.text = "Awaiting Player2";
		}
		else
		{
			Debug.Log("==Deploying player2==");
			PhotonNetwork.Instantiate(player2, player2Transform.position, Quaternion.identity, 0);
			this.photonView.RPC("StartGame", RpcTarget.All);
		}
	}

	[PunRPC]
	public void StartGame()
	{
		Debug.Log("==Starting Game==");
		this.gameStatusUI.text = "3";
		this.IsGameStarting = true;
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("==Join Room failed. Attempting to Create a room==");
		PhotonNetwork.CreateRoom(roomName);
	}
}
