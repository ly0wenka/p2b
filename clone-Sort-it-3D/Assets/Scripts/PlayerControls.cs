using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class PlayerControls : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;
    private SpriteRenderer spriteRenderer;

    private Vector3Int Direction;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        PhotonPeer.RegisterType(typeof(Vector3Int), 
            242, 
            SerializeVector3Int, 
            DeserializeVector3Int);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log(nameof(KeyCode.LeftArrow));
                Direction = Vector3Int.left;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Direction = Vector3Int.right;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Direction = Vector3Int.up;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Direction = Vector3Int.down;
            }

        }

        if (Direction == Vector3Int.left)
        {
            spriteRenderer.flipX = false;
        }

        if (Direction == Vector3Int.right)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Direction);
        }
        else
        {
            Direction = (Vector3Int)stream.ReceiveNext();
        }
    }

    public static object DeserializeVector3Int(byte[] data) =>
        new Vector3Int
        {
            x = BitConverter.ToInt32(data, 0), 
            y = BitConverter.ToInt32(data, 4), 
            z = BitConverter.ToInt32(data, 8)
        };

    public static byte[] SerializeVector3Int(object obj)
    {
        var vector = new Vector3Int();
        var result = new byte[12];
        
        BitConverter.GetBytes(vector.x).CopyTo(result, 0);
        BitConverter.GetBytes(vector.y).CopyTo(result, 4);
        BitConverter.GetBytes(vector.z).CopyTo(result, 8);
        
        return result;
    }
}
