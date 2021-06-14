using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OpponentAI : MonoBehaviour
{
    private Transform _opponentTransform;
    private CharacterController _opponentController;

    private Animation _opponentAnim;

    public AnimationClip _opponentIdleAnim;

    private Vector3 _opponentMoveDirection = Vector3.zero;

    private float _opponentGravity = 5f;
    private float _opponentGravityModifier = 5f;
    private float _opponentVerticalSpeed = .0f;

    private CollisionFlags _collisionFlagsOpponent;

    private OpponentAIState _opponentAIState;
    
    // Start is called before the first frame update
    void Start()
    {
        _opponentController = GetComponent<CharacterController>();

        _opponentAnim = GetComponent<Animation>();

        _opponentTransform = transform;
        
        _opponentMoveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyOpponentGravity();
    }

    private void ApplyOpponentGravity()
    {
        
    }
    
    public bool OpponentIsGravity => (_collisionFlagsOpponent & CollisionFlags.Below) != 0;
}
