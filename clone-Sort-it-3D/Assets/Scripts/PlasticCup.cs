using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class PlasticCup : MonoBehaviour
{
    // private PhotonView photonView;
    // public Vector3Int Direction;
    //
    // private void Start()
    // {
    //     photonView = GetComponent<PhotonView>();
    //     PhotonPeer.RegisterType(typeof(Vector3Int), 
    //         242, 
    //         Ball.SerializeVector3Int, 
    //         Ball.DeserializeVector3Int);
    // }
    //
    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.IsWriting)
    //     {
    //         stream.SendNext(Direction);
    //     }
    //     else
    //     {
    //         Direction = (Vector3Int)stream.ReceiveNext();
    //     }
    // }
}
