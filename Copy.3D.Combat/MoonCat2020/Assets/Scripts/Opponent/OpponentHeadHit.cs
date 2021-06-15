using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHeadHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    private void HeadStruck()
    {
        OpponentAI._opponentAIState = OpponentAIState.OpponentHitHead;
    }
}
