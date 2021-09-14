using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentLowKick : MonoBehaviour
{
    public static Vector3 _playerImpactPoint;
    
    public float _nextHighKickIsAllowed = -1.0f;
    public float _attackDelay = 1.0f;

    private Collider _headHitCollider;

    private bool _returnIfOpponentIsHighKicking;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
