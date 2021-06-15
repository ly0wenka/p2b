using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHeadHit : MonoBehaviour
{
    public static Vector3 _opponentImpactPoint;
    // Start is called before the first frame update
    void Start()
    {
        _opponentImpactPoint = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        OpponentAI._opponentAIState = OpponentAIState.OpponentHitHead;
    }
}
