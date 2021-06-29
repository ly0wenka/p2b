using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject generatorNewRoundPrefab;
    // Start is called before the first frame update
    void Start()
    {
        var pos = new Vector3(Random.Range(-5, 5),Random.Range(-5, 5));
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
        PhotonNetwork.Instantiate(generatorNewRoundPrefab.name, Vector3.zero, Quaternion.identity);
        PhotonPeer.RegisterType(typeof(SyncData), 
            242, 
            SyncData.Serialize, 
            SyncData.Deserialize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(nameof(OnPlayerEnteredRoom));
        if (PhotonNetwork.IsMasterClient)
        {
            generatorNewRoundPrefab.GetComponent<GeneratorNewRound>().SendSyncData(newPlayer);
        }
        Debug.Log($"Player {newPlayer.NickName} entered room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left room");
    }
}
