using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviour, IPunObservable
{
    public bool MoveUp = false;
    public bool Move = false;

    public float progressup;
    public Vector3 MoveToUp;

    private float progress;
    public Vector3 MoveTo;

    public float step;

    [HideInInspector] public bool AlreadyGetColor = false;

    private PhotonView photonView;
    private Renderer renderer;
    private Vector3 tempcolor;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        renderer = GetComponent<Renderer>();
        //Direction = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        PhotonPeer.RegisterType(typeof(Vector3Int),
            242,
            SerializeVector3Int,
            DeserializeVector3Int);

        PhotonPeer.RegisterType(typeof(Color),
            242,
            SerializeColor,
            DeserializeColor);
    }

    private object DeserializeColor(byte[] data)
        =>
            new Color
            {
                r = BitConverter.ToSingle(data, 0),
                g = BitConverter.ToSingle(data, 4),
                b = BitConverter.ToSingle(data, 8),
                a = BitConverter.ToSingle(data, 12),
            };

    private byte[] SerializeColor(object customobject)
    {
        var color = new Color();
        var result = new byte[16];

        BitConverter.GetBytes(color.r).CopyTo(result, 0);
        BitConverter.GetBytes(color.g).CopyTo(result, 4);
        BitConverter.GetBytes(color.b).CopyTo(result, 8);
        BitConverter.GetBytes(color.a).CopyTo(result, 12);

        return result;
    }

    void FixedUpdate()
    {
        //if (photonView.IsMine)
        {
            if (MoveTo != null)
            {
                if (MoveUp == true)
                {
                    transform.position = Vector3.Lerp(transform.position, MoveToUp, step);
                    progressup += step;
                }

                if (Move == true && progressup >= 3)
                {
                    transform.position = Vector3.Lerp(transform.position, MoveTo, step);
                    progress += step;
                    MoveUp = false;
                }

                if (progress >= 3)
                {
                    Move = false;
                    progress = 0;
                    progressup = 0;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            tempcolor = new Vector3(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b);

            stream.Serialize(ref tempcolor);
            stream.Serialize(ref MoveUp);
            stream.Serialize(ref Move);
            stream.Serialize(ref progressup);
            stream.Serialize(ref MoveToUp);
            stream.Serialize(ref progress);
            stream.Serialize(ref MoveTo);
            stream.Serialize(ref step);
            //stream.SendNext(transform.GetComponent<Renderer>().material.color);
        }
        else
        {
            stream.Serialize(ref tempcolor);
            stream.Serialize(ref MoveUp);
            stream.Serialize(ref Move);
            stream.Serialize(ref progressup);
            stream.Serialize(ref MoveToUp);
            stream.Serialize(ref progress);
            stream.Serialize(ref MoveTo);
            stream.Serialize(ref step);

            renderer.material.color = new Color(tempcolor.x, tempcolor.y, tempcolor.z, 1.0f);
            //transform.GetComponent<Renderer>().material.color = (Color)stream.ReceiveNext();
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