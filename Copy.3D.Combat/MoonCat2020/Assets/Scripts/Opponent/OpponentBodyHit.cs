using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentBodyHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider _opponentBodyHit)
    {
        if (_opponentBodyHit.CompareTag(("BodyHit")))
        {
            BodyStruck();
        }
    }

    private void BodyStruck()
    {
        OpponentAI._opponentAIState = OpponentAIState.OpponentHitBody;
    }
}
