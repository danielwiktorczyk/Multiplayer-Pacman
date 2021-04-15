using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherUIController : MonoBehaviour
{
    private const string playerNamePrefKey = "PlayerName";

    [SerializeField] private Text playerNameLabel;

    private void Awake()
    {
        this.playerNameLabel.text = $"Player name: {PlayerPrefs.GetString(playerNamePrefKey)}";
    }

    public void SetPlayerName(string playerName)
    {
        Debug.Log($"SetPlayerName Called");

        if (string.IsNullOrWhiteSpace(playerName))
            return;

        Debug.Log($"Player name set to {playerName}");
        PlayerPrefs.SetString(playerNamePrefKey, playerName);
        this.playerNameLabel.text = $"Player name: {playerName}";
        PhotonNetwork.NickName = playerName;
    }

}
