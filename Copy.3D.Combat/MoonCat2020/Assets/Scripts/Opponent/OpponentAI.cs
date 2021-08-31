using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class OpponentAI : MonoBehaviour
{
    private Transform _opponentTransform;
    private CharacterController _opponentController;

    #region Rotation

    public static GameObject _playerOne;
    public static GameObject _opponent;

    private Vector3 _playersPosition;
    private Vector3 _opponentPosition;
    private Vector3 _positionDifference;
    public float _positionDifferenceModifier = 2.0f; 

    private Quaternion _targetRotation;
    private int _defaultRotation = 0;
    private int _alternativeRotation = 180;
    public float _rotationSpeed = 5f;
    public float _opponentWalkSpeed = 1.0f;

    #endregion

    private Animator _opponentAnimator;

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

    private AudioSource _opponentAIAudioSource;
    public AudioClip _opponentHeadHitAudio;
    public AudioClip _opponentBodyHitAudio;

    public GameObject _hitSparks;

    private Vector3 _opponentMoveDirection = Vector3.zero;

    public float _opponentJumpHeight = .5f;
    public float _opponentJumpSpeed = 1f;
    public float _opponentJumpHorizontal = 1f;
    private Vector3 _jumpHeightTemp;

    private float _opponentGravity = 5f;
    private float _opponentGravityModifier = 5f;
    private float _opponentVerticalSpeed = .0f;

    public RangeInt _offensivePriority = new RangeInt(1, 3);
    public RangeInt _assessPriority = new RangeInt(4, 6);
    public RangeInt _defensivePriority = new RangeInt(7, 9);

    private int _decideForwardMovement;
    private int _decideBackwardMovement;

    private int _minimumDecideValue;
    private int _maximumDecideValue;
    public int _tippingPointDecideValue;
    private int _decideAggressionPriority;

    private bool _assessingThePlayer;
    public float _assessingTime = 3;
    private bool _returnFightIntroFinished;

    private CollisionFlags _collisionFlagsOpponent;

    public static OpponentAIState _opponentAIState;

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

        StartCoroutine(OpponentFSM());
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

    private IEnumerator OpponentFSM()
    {
        while (true)
        {
            switch (_opponentAIState)
            {
                case OpponentAIState.Initialise:
                    Initialise();
                    break;
                case OpponentAIState.OpponentIdle:
                    OpponentIdle();
                    break;
                case OpponentAIState.OpponentHitByLowKick:
                    OpponentHitByLowKick();
                    break;
                case OpponentAIState.OpponentHitByLeftPunch:
                    OpponentHitByLeftPunch();
                    break;
                case OpponentAIState.WaitForHitAnimations:
                    WaitForHitAnimations();
                    break;
                case OpponentAIState.DefeatedFinalHit:
                    OpponentDefeated();
                    break;
                case OpponentAIState.OpponentHitByRightPunch:
                    OpponentHitByRightPunch();
                    break;
                case OpponentAIState.OpponentHitByHighKick:
                    OpponentHitByHighKick();
                    break;
                case OpponentAIState.AdvanceOnThePlayer:
                    AdvanceOnThePlayer();
                    break;
                case OpponentAIState.RetreatFromThePlayer:
                    RetreatFromThePlayer();
                    break;
                case OpponentAIState.WalkTowardsThePlayer:
                    WalkTowardsThePlayer();
                    break;
                case OpponentAIState.WalkAwayFromThePlayer:
                    WalkAwayFromThePlayer();
                    break;
                case OpponentAIState.JumpTowardsThePlayer:
                    JumpTowardsThePlayer();
                    break;
                case OpponentAIState.JumpAwayThePlayer:
                    JumpAwayThePlayer();
                    break;
                case OpponentAIState.OpponentJumpUp:
                    OpponentJumpUp();
                    break;
                case OpponentAIState.OpponentComeDown:
                    OpponentComeDown();
                    break;
                case OpponentAIState.OpponentComeDownForward:
                    OpponentComeDownForward();
                    break;
                case OpponentAIState.OpponentComeDownBackwards:
                    OpponentComeDownBackwards();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return null;
        }
    }

    private void OpponentJumpAnimation()
    {
        Debug.Log(nameof(OpponentJumpAnimation));

        _opponentAnimator.CrossFade(_opponentJumpAnim.name);
    }
    
    private void OpponentPunchLeftAnimation()
    {
        Debug.Log(nameof(OpponentPunchLeftAnimation));
        
        _opponentAnimator.CrossFade(_opponentPunchLeftAnim.name);    
    }
    private void OpponentPunchRightAnimation()
    {
        Debug.Log(nameof(OpponentPunchRightAnimation));
        
        _opponentAnimator.CrossFade(_opponentPunchRightAnim.name);    
    }
    private void OpponentKickHighAnimation()
    {
        Debug.Log(nameof(OpponentKickHighAnimation));
        
        _opponentAnimator.CrossFade(_opponentKickHighAnim.name);    
    }
    private void OpponentKickLowAnimation()
    {
        Debug.Log(nameof(OpponentKickLowAnimation));
        
        _opponentAnimator.CrossFade(_opponentKickLowAnim.name);    
    }

    #region Jump

    private void JumpTowardsThePlayer()
    {
        Debug.Log(nameof(JumpTowardsThePlayer));

        OpponentJumpAnimation();

        _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
        _opponentMoveDirection *= _opponentJumpSpeed;

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

        if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _opponentAIState = OpponentAIState.OpponentComeDownForward;
        }
    }

    private void JumpAwayThePlayer()
    {
        Debug.Log(nameof(JumpAwayThePlayer));

        OpponentJumpAnimation();
        _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
        _opponentMoveDirection *= _opponentJumpSpeed;

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

        if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _opponentAIState = OpponentAIState.OpponentComeDownBackwards;
        }
    }

    private void OpponentJumpUp()
    {
        Debug.Log(nameof(OpponentJumpUp));

        OpponentJumpAnimation();
        _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
        _opponentMoveDirection *= _opponentJumpSpeed;

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

        if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _opponentAIState = OpponentAIState.OpponentComeDown;
        }
    }

    private void OpponentComeDown()
    {
        Debug.Log(nameof(OpponentComeDown));

        //OpponentJumpAnimation();
        _opponentMoveDirection = new Vector3(0, _opponentVerticalSpeed, 0);
        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection);
        _opponentMoveDirection *= _opponentJumpSpeed;

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (OpponentIsGrounded())
        {
            _opponentAIState = OpponentAIState.OpponentIdle;
        }
    }

    private void OpponentComeDownForward()
    {
        Debug.Log(nameof(OpponentComeDownForward));

        //OpponentJumpAnimation();
        _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
        _opponentMoveDirection *= _opponentJumpSpeed;

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

        if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _opponentAIState = OpponentAIState.OpponentIdle;
        }
    }

    private void OpponentComeDownBackwards()
    {
        Debug.Log(nameof(OpponentComeDownBackwards));

        //OpponentJumpAnimation();
        _opponentMoveDirection = new Vector3(0, _opponentJumpSpeed, 0);
        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection).normalized;
        _opponentMoveDirection *= _opponentJumpSpeed;

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);

        if (_opponentTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _opponentAIState = OpponentAIState.OpponentIdle;
        }
    }

    #endregion

    private void OpponentIdle()
    {
        Debug.Log(nameof(OpponentIdle));

        if (OpponentIsGrounded())
        {
            return;
        }

        OpponentIdleAnimation();

        OpponentGravityIdle();

        _returnFightIntroFinished = FightIntro._fightIntroFinished;

        if (_returnFightIntroFinished != true)
        {
            return;
        }

        if (_decideAggressionPriority < _assessPriority.start)
        {
            _opponentAIState = OpponentAIState.AdvanceOnThePlayer;
        }

        if (_decideAggressionPriority > _offensivePriority.end
            && _decideAggressionPriority < _defensivePriority.start)
        {
            if (_assessingThePlayer)
            {
                return;
            }

            _assessingThePlayer = true;
            StartCoroutine(AssessThePlayer());
        }

        if (_decideAggressionPriority == _defensivePriority.start)
        {
            _opponentAIState = OpponentAIState.RetreatFromThePlayer;
        }
    }

    private void OpponentGravityIdle()
    {
        _opponentMoveDirection = new Vector3(0, _opponentVerticalSpeed, 0);

        _opponentMoveDirection = _opponentTransform.TransformDirection(_opponentMoveDirection);

        _collisionFlagsOpponent = _opponentController.Move(_opponentMoveDirection * Time.deltaTime);
    }

    private void OpponentIdleAnimation()
    {
        Debug.Log(nameof(OpponentIdleAnimation));

        _opponentAnimator.CrossFade(_opponentIdleAnim.name);
    }

    private void Initialise()
    {
        Debug.Log(nameof(Initialise));

        _decideAggressionPriority = 2; //Random.Range(1, 9);

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void InitialiseAnimation()
    {
        Debug.Log(nameof(InitialiseAnimation));
    }

    private void AdvanceOnThePlayer()
    {
        Debug.Log(nameof(AdvanceOnThePlayer));

        _decideForwardMovement = Random.Range(1, 10);

        if (_decideBackwardMovement >= _minimumDecideValue
            && _decideForwardMovement <= _tippingPointDecideValue)
        {
            _opponentAIState = OpponentAIState.WalkTowardsThePlayer;
        }

        if (_decideBackwardMovement <= _maximumDecideValue
            && _decideForwardMovement > _tippingPointDecideValue)
        {
            if (_positionDifference.x >= _positionDifferenceModifier)
            {
                _opponentAIState = OpponentAIState.JumpTowardsThePlayer;
            }
            else
            {
                _opponentAIState = OpponentAIState.WalkTowardsThePlayer; 
            }
        }
    }

    private void RetreatFromThePlayer()
    {
        Debug.Log(nameof(RetreatFromThePlayer));

        _decideForwardMovement = Random.Range(1, 10);

        if (_decideBackwardMovement >= _minimumDecideValue
            && _decideForwardMovement <= _tippingPointDecideValue)
        {
        }

        if (_decideBackwardMovement <= _maximumDecideValue
            && _decideForwardMovement > _tippingPointDecideValue)
        {
        }
    }

    private void WalkAwayFromThePlayer()
    {
        Debug.Log(nameof(WalkAwayFromThePlayer));

        _opponentMoveDirection =
            (transform.position - _playersPosition)
            * _opponentWalkSpeed;

        _opponentMoveDirection.Normalize();

        _opponentMoveDirection.y = 0;
        _opponentMoveDirection.z = 0;

        _collisionFlagsOpponent =
            _opponentController.Move(
                _opponentMoveDirection * Time.deltaTime);

        OpponentWalkAnimation();
    }

    private void WalkTowardsThePlayer()
    {
        Debug.Log(nameof(WalkTowardsThePlayer));

        _opponentMoveDirection =
            (_playersPosition - transform.position)
            * _opponentWalkSpeed;

        _opponentMoveDirection.Normalize();

        _opponentMoveDirection.y = 0;
        _opponentMoveDirection.z = 0;

        _collisionFlagsOpponent =
            _opponentController.Move(
                _opponentMoveDirection * Time.deltaTime);

        OpponentWalkAnimation();
    }

    private void OpponentWalkAnimation()
    {
        Debug.Log(nameof(OpponentWalkAnimation));

        _opponentAnimator.CrossFade(_opponentWalkAnim.name);
    }

    private IEnumerator AssessThePlayer()
    {
        Debug.Log(nameof(AssessThePlayer));

        yield return new WaitForSeconds(_assessingTime);

        _assessingThePlayer = false;
    }

    private void OpponentHitByLowKick()
    {
        Debug.Log(nameof(OpponentHitByLowKick));

        OpponentHitHeadAnimation();

        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerLowKick._opponentImpactPoint;

        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitByHighKick()
    {
        Debug.Log(nameof(OpponentHitByHighKick));

        OpponentHitHeadAnimation();

        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerHighKick._opponentImpactPoint;

        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitBodyAnimation()
    {
        Debug.Log(nameof(OpponentHitBodyAnimation));

        _opponentAnimator.CrossFade(_opponentHitBodyAnim.name);
    }

    public float _temp = 0;

    private void OpponentHitByLeftPunch()
    {
        Debug.Log(nameof(OpponentHitByLeftPunch));

        OpponentHitHeadAnimation();

        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerPunchLeft._opponentImpactPoint;

        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitByRightPunch()
    {
        Debug.Log(nameof(OpponentHitByRightPunch));

        OpponentHitHeadAnimation();

        _opponentAIAudioSource.PlayOneShot(_opponentHeadHitAudio);

        var _impactPoint = PlayerPunchRight._opponentImpactPoint;

        var _hs = Instantiate(_hitSparks, new Vector3(_impactPoint.x, _impactPoint.y, _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _opponentAIState = OpponentAIState.WaitForHitAnimations;
    }

    private void OpponentHitHeadAnimation()
    {
        Debug.Log(nameof(OpponentHitHeadAnimation));

        _opponentAnimator.CrossFade(_opponentHitHeadAnim.name);
    }

    private void WaitForHitAnimations()
    {
        Debug.Log(nameof(WaitForHitAnimations));
    }

    private void WaitForHitAnimationsAnimation()
    {
        Debug.Log(nameof(WaitForHitAnimationsAnimation));

        if (_opponentAnimator.IsPlaying(_opponentHitBodyAnim.name))
        {
            return;
        }

        if (_opponentAnimator.IsPlaying(_opponentHitHeadAnim.name))
        {
            return;
        }

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void OpponentDefeated()
    {
        Debug.Log(nameof(OpponentDefeated));

        OpponentGravityIdle();

        if (_opponentAnimator.IsPlaying(_opponentDefeatedFinalHitAnim.name))
        {
            return;
        }

        StopCoroutine(OpponentFSM());
    }

    private void SetOpponentDefeated()
    {
        Debug.Log(nameof(SetOpponentDefeated));

        OpponentFinalHitAnimation();

        _opponentAIState = OpponentAIState.OpponentIdle;
    }

    private void OpponentFinalHitAnimation()
    {
        Debug.Log(nameof(OpponentFinalHitAnimation));

        _opponentAnimator.CrossFade(_opponentDefeatedFinalHitAnim.name);
    }

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
        Debug.Log(nameof(UpdatePlayerPosition));

        _playersPosition = new Vector3(_playerOne.transform.position.x, _playerOne.transform.position.y,
            _playerOne.transform.position.z);
    }

    private void UpdateOpponentsPosition()
    {
        Debug.Log(nameof(UpdateOpponentsPosition));

        _opponentPosition = new Vector3(_opponent.transform.position.x, _opponent.transform.position.y,
            _opponent.transform.position.z);
    }

    private void UpdatePositionDifference()
    {
        Debug.Log(nameof(UpdateOpponentsPosition));

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
        Debug.Log(nameof(UpdateOpponentsRotation));

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
        Debug.Log(nameof(UpdateOpponentsPlanePosition));

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