using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHeadHit : MonoBehaviour
{
    public static Vector3 _opponentImpactPoint;

    private Collider _headHitCollider;

    private bool _returnIfPlayerIsPunching;
    // Start is called before the first frame update
    void Start()
    {
        _opponentImpactPoint = Vector3.zero;

        _headHitCollider = GetComponent<Collider>();
        _headHitCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _returnIfPlayerIsPunching = PlayerOneMovement._playerIsPunchingLeft;

        if (_returnIfPlayerIsPunching)
        {
            _headHitCollider.enabled = true;
        }

        if (!_returnIfPlayerIsPunching)
        {
            _headHitCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider _opponentHeadHit)
    {
        if (_opponentHeadHit.CompareTag(("HeadHit")))
        {
            HeadStruck();
        }

        _opponentHeadHit.ClosestPointOnBounds(transform.position);

        _opponentImpactPoint = _opponentHeadHit.transform.position;
    }

    private void HeadStruck()
    {
        OpponentAI._opponentAIState = OpponentAIState.OpponentHitByLeftPunch;
    }
}
