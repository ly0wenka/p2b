using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text LogText;
    // Start is called before the first frame update
    void Start()
    {
        Log($"Player's name is set to {PhotonNetwork.NickName}");
        
        PhotonNetwork.NickName = "player " + Random.Range(1000, 9999);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect To master");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = 2});
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Log(nameof(OnJoinedRoom));
        
        PhotonNetwork.LoadLevel("Game");
    }

    private void Log(string message)
    {
        Debug.Log(message);
        LogText.text += $"{Environment.NewLine}{message}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
