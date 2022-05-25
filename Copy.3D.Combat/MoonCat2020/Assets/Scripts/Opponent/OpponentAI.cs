using System;
using System.Collections;
using System.Collections.Generic;
using Opponent.OpponentAIState;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class OpponentAI : MonoBehaviour
{
    #region fieldsStartUpdate

    protected Transform _opponentTransform;
    protected CharacterController _opponentController;

    #region Rotation

    public static GameObject _playerOne;
    public static GameObject _opponent;

    private protected Vector3 _playersPosition;
    private Vector3 _opponentPosition;
    protected Vector3 _positionDifference;
    public float _positionDifferenceModifier = 2.0f; 

    private Quaternion _targetRotation;
    private int _defaultRotation = 0;
    private int _alternativeRotation = 180;
    public float _rotationSpeed = 5f;
    public float _opponentWalkSpeed = 1.0f;

    #endregion

    protected Animator _opponentAnimator;

    public AnimationClip _opponentIdleAnim;
    public AnimationClip _opponentHitBodyAnim;
    public AnimationClip _opponentHitHeadAnim;
    public AnimationClip _opponentDefeatedFinalHitAnim;
    public AnimationClip _opponentWalkAnim;
    public AnimationClip _opponentJumpAnim;
    
    public AnimationClip _opponentPunchLeftAnim;
    public AnimationClip _opponentPunchRightAnim;
    public AnimationClip _opponentKickHighAnim;
    public AnimationClip _opponentKickLowAnim;

    protected AudioSource _opponentAIAudioSource;
    public AudioClip _opponentHeadHitAudio;
    public AudioClip _opponentBodyHitAudio;

    public GameObject _hitSparks;

    protected Vector3 _opponentMoveDirection = Vector3.zero;

    public float _opponentJumpHeight = .5f;
    public float _opponentJumpSpeed = 1f;
    public float _opponentJumpHorizontal = 1f;
    protected Vector3 _jumpHeightTemp;

    private float _opponentGravity = 5f;
    private float _opponentGravityModifier = 5f;
    protected float _opponentVerticalSpeed = .0f;

    public RangeInt _offensivePriority = new RangeInt(1, 3);
    public RangeInt _assessPriority = new RangeInt(4, 6);
    public RangeInt _defensivePriority = new RangeInt(7, 9);

    protected int _decideForwardMovement;
    protected int _decideBackwardMovement;

    protected int _minimumDecideValue;
    protected int _maximumDecideValue;
    public int _tippingPointDecideValue;
    protected int _decideAggressionPriority;

    protected bool _assessingThePlayer;
    public float _assessingTime = 3;

    protected ChooseAttack _chooseAttack;
    protected int _switchAttackValue;
    protected int _switchAttackStateValue;
    public int _punchKickPivotValue;
    public float _chooseAttackDistanceModifier = .75f;
    
    public int _leftPunchRangeMin, _leftPunchRangeMax;
    public int _rightPunchRangeMin, _rightPunchRangeMax;
    public int _lowKickRangeMin, _lowKickRangeMax;
    public int _highKickRangeMin, _highKickRangeMax;

    public static bool _ooponentIsPunchingLeft;
    public static bool _opponentIsPunchingRight;
    public static bool _opponentIsKickingLow;
    public static bool _ooponentIsKickingHigh;

    protected bool _returnFightIntroFinished;

    protected CollisionFlags _collisionFlagsOpponent;

    public static IOpponentAIState _opponentAIState = new OpponentAIStateStateInitialise();

    protected enum ChooseAttack
    {
        LeftPunch,
        RightPunch,
        LowKick,
        HighKick
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _opponentController = GetComponent<CharacterController>();

        _opponentAnimator = GetComponent<Animator>();

        _opponentAIAudioSource = GetComponent<AudioSource>();

        _opponentTransform = transform;

        _opponentMoveDirection = Vector3.zero;

        _jumpHeightTemp = new Vector3(0, _opponentJumpHeight, 0);

        _decideAggressionPriority = 0;

        _assessingThePlayer = false;

        _minimumDecideValue = 1;
        _maximumDecideValue = 10;

        OpponentIsMovesInit();

        StartCoroutine(OpponentFSM());
    }

    private protected void OpponentIsMovesInit()
    {
        _ooponentIsPunchingLeft = false;
        _opponentIsPunchingRight = false;
        _opponentIsKickingLow = false;
        _ooponentIsKickingHigh = false;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyOpponentGravity();

        UpdatePlayerPosition();
        UpdateOpponentsPosition();
        UpdateOpponentsRotation();
        UpdatePositionDifference();
        UpdateOpponentsPlanePosition();
    }
    #endregion

    protected IEnumerator OpponentFSM()
    {
        while (true)
        {
            _opponentAIState.Do();

            yield return null;
        }
    }

    protected void OpponentJumpAnimation()
    {
        // Debug.Log(nameof(OpponentJumpAnimation));

        _opponentAnimator.CrossFade(_opponentJumpAnim.name);
    }

    protected void OpponentPunchLeftAnimation()
    {
        // Debug.Log(nameof(OpponentPunchLeftAnimation));
        
        _opponentAnimator.CrossFade(_opponentPunchLeftAnim.name);    
    }

    protected void OpponentPunchRightAnimation()
    {
        // Debug.Log(nameof(OpponentPunchRightAnimation));
        
        _opponentAnimator.CrossFade(_opponentPunchRightAnim.name);    
    }
    private void OpponentKickHighAnimation()
    {
        // Debug.Log(nameof(OpponentKickHighAnimation));
        
        _opponentAnimator.CrossFade(_opponentKickHighAnim.name);    
    }

    protected void OpponentKickLowAnimation()
    {
        // Debug.Log(nameof(OpponentKickLowAnimation));
        
        _opponentAnimator.CrossFade(_opponentKickLowAnim.name);    
    }

    #region Jump

    #endregion

    private void OpponentIdle()
    {
        
    }

    protected void OpponentGravityIdle()
    {
        // Debug.Log(nameof(OpponentGravityIdle));

        _opponentMoveDirection = new Vector3(0, _opponentVerticalSpeed, 0);

        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection);

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);
    }

    protected void OpponentIdleAnimation()
    {
        // Debug.Log(nameof(OpponentIdleAnimation));

        _opponentAnimator.CrossFade(_opponentIdleAnim.name);
    }

    private void InitialiseAnimation()
    {
        Debug.Log(nameof(InitialiseAnimation));
    }

    private protected void OpponentWalkAnimation()
    {
        // Debug.Log(nameof(OpponentWalkAnimation));

        _opponentAnimator.CrossFade(_opponentWalkAnim.name);
    }

    protected IEnumerator AssessThePlayer()
    {
        // Debug.Log(nameof(AssessThePlayer));

        yield return new WaitForSeconds(_assessingTime);

        _assessingThePlayer = false;
    }

    private protected void OpponentHitBodyAnimation()
    {
        // Debug.Log(nameof(OpponentHitBodyAnimation));

        _opponentAnimator.CrossFade(_opponentHitBodyAnim.name);
    }

    public float _temp = 0;

    private protected void OpponentHitHeadAnimation()
    {
        // Debug.Log(nameof(OpponentHitHeadAnimation));

        _opponentAnimator.CrossFade(_opponentHitHeadAnim.name);
    }

    #region PunchKick
    #endregion

    private void DefeatedFinalHit()
    {
        Debug.Log(nameof(DefeatedFinalHit));
    }

    private void DefeatedFinalHitAnimation()
    {
        Debug.Log(nameof(DefeatedFinalHitAnimation));
    }

    #region Update

    private void UpdatePlayerPosition()
    {
        // Debug.Log(nameof(UpdatePlayerPosition));

        _playersPosition = new Vector3(_playerOne.transform.position.x, _playerOne.transform.position.y,
            _playerOne.transform.position.z);
    }

    private void UpdateOpponentsPosition()
    {
        // Debug.Log(nameof(UpdateOpponentsPosition));

        _opponentPosition = new Vector3(_opponent.transform.position.x, _opponent.transform.position.y,
            _opponent.transform.position.z);
    }

    private void UpdatePositionDifference()
    {
        // Debug.Log(nameof(UpdateOpponentsPosition));

        if (_playerOne.transform.position.x < _opponent.transform.position.x)
        {
            _positionDifference = (_opponentPosition - _playersPosition);
        }

        if (_playerOne.transform.position.x >= _opponent.transform.position.x)
        {
            _positionDifference = (_playersPosition - _opponentPosition);
        }
    }

    private void UpdateOpponentsRotation()
    {
        // Debug.Log(nameof(UpdateOpponentsRotation));

        if (_playerOne.transform.position.x < _opponent.transform.position.x)
        {
            if (_opponent.transform.rotation.y == _defaultRotation)
            {
                return;
            }
            else
            {
                _targetRotation = Quaternion.Euler(0, _defaultRotation, 0);

                _opponent.transform.rotation =
                    Quaternion.Slerp(
                        transform.rotation,
                        _targetRotation,
                        Time.deltaTime * _rotationSpeed);
            }
        }

        if (_playerOne.transform.position.x > _opponent.transform.position.x)
        {
            if (Math.Abs(_opponent.transform.rotation.y - _alternativeRotation) < 0.001)
            {
                return;
            }
            else
            {
                _targetRotation = Quaternion.Euler(0, _alternativeRotation, 0);

                _opponent.transform.rotation =
                    Quaternion.Slerp(
                        transform.rotation,
                        _targetRotation,
                        Time.deltaTime * _rotationSpeed);

                Debug.LogWarning("TEST");
            }
        }
    }

    private void UpdateOpponentsPlanePosition()
    {
        // Debug.Log(nameof(UpdateOpponentsPlanePosition));

        if (_opponentController.transform.position.z != GameManager._opponentsStartingPosition.z)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                GameManager._opponentsStartingPosition.z);
        }
    }

    #endregion

    private void ApplyOpponentGravity()
    {
        if (OpponentIsGrounded())
        {
            _opponentVerticalSpeed = .0f;
        }
        else
        {
            _opponentVerticalSpeed -= _opponentGravity * _opponentGravityModifier * Time.deltaTime;
        }
    }

    public bool OpponentIsGrounded() => (_collisionFlagsOpponent & CollisionFlags.Below) != 0;
}