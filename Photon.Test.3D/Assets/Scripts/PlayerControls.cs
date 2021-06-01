using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerControls : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;
    private SpriteRenderer spriteRenderer;

    private bool isRed;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-Time.deltaTime * 5, 0, 0);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Time.deltaTime * 5, 0, 0);
            }

            isRed = Input.GetKey(KeyCode.Space);
        }

        spriteRenderer.color = isRed ? Color.red : Color.white;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isRed);
        }
        else
        {
            isRed = (bool)stream.ReceiveNext();
        }
    }
}
