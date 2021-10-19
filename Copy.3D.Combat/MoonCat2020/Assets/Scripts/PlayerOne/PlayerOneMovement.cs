using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerOneMovement : MonoBehaviour
{
    private Transform _playerOneTransform;
    private CharacterController _playerController;

    #region Rotation

    public static GameObject _playerOne;
    public static GameObject _opponent;

    private Vector3 _playersPosition;
    private Vector3 _opponentPosition;

    private Quaternion _targetRotation;
    private int _defaultRotation = 0;
    private int _alternativeRotation = 180;
    public float _rotationSpeed = 5f;

    #endregion

    public float _playerWalkSpeed = 1f;
    public float _playerRetreatSpeed = .75f;
    public float _playerJumpHeight = 0.5f;
    public float _playerJumpSpeed = 0.5f;
    public float _playerJumpHorizontal = 0.5f;
    private Vector3 _jumpHeightTemp;

    private Animator _playerOneAnimator;

    public AnimationClip _playerOneIdleAnim;
    public AnimationClip _playerOneWalkAnim;
    public AnimationClip _playerOneJumpAnim;
    public AnimationClip _playerOneDemoAnim;
    public AnimationClip[] _playerAttackAnim;
    public AnimationClip _playerHitBodyAnim;
    public AnimationClip _playerHitHeadAnim;
    public AnimationClip _playerDefeatedFinalHitAnim;

    private AudioSource _playerAudioSource;
    public AudioClip _playerHeadHitAudio;
    public AudioClip _playerBodyHitAudio;

    public GameObject _hitSparks;

    public static bool _playerIsPunchingLeft;
    public static bool _playerIsPunchingRight;
    public static bool _playerIsKickingLow;
    public static bool _playerIsKickingHigh;
    
    public float _controllerDeadZonePos = .1f;
    public float _controllerDeadZoneNeg = -.1f;

    private float _xAxis;
    private float _yAxis;
    private float _analogStickAngle;

    private int _0DegreesAngle = 0;
    private int _90DegreesAngle = 90;
    private int _180DegreesAngle = 180;
    private float _degreeModifier = 22.5f;

    public float _playersGravity = 20f;
    public float _playerGravityModifier = 5f;
    public float _playersSpeedYAxis;

    private bool _returnDemoState;
    private int _demoRotationValue = 75;

    private bool _returnFightIntroFinished;

    private Vector3 _playerOneMoveDirection = Vector3.zero;

    private CollisionFlags _collisionFlags;

    public static PlayerOneStates _playerOneStates;

    void Start()
    {
        _playerOneTransform = transform;

        _playerOneMoveDirection = Vector3.zero;
        
        _jumpHeightTemp = new Vector3(0, _playerJumpHeight, 0);

        _playersSpeedYAxis = 0;

        _playerController = GetComponent<CharacterController>();

        _playerOneAnimator = GetComponent<Animator>();

        _playerAudioSource = GetComponent<AudioSource>();

        // foreach (var animClip in _playerAttackAnim)
        // {
        //     _playerOneAnimator[animClip.name].wrapMode = WrapMode.Once;
        // }

        StartCoroutine(nameof(PlayerOneFSM));

        _returnDemoState = false;

        _returnDemoState = ChooseCharacter._demoPlayer;

        _playerIsPunchingLeft = false;
        _playerIsPunchingRight = false;
        _playerIsKickingLow = false;
        _playerIsKickingHigh = false;
        
        if (_returnDemoState)
        {
            _playerOneStates = PlayerOneStates.PlayerDemo;
        }
    }

    void Update()
    {
        ApplyGravity();
        PlayerInputController();

        if (_playerAttackAnim.Any(clip => _playerOneAnimator.IsPlaying(clip.name)))
        {
            return;
        }

        _returnFightIntroFinished = FightIntro._fightIntroFinished;

        if (_returnFightIntroFinished != true)
        {
            return;
        }

        if (PlayerOneIsGrounded())
        {
            HorizontalJumpInputManager();
            AttackInputManager();
            StandardInputManager();
        }
        
        UpdatePlayerPosition();
        UpdateOpponentsPosition();
        UpdatePlayersRotation();
        UpdatePlayersPlanePosition();
    }

    private IEnumerator PlayerOneFSM()
    {
        while (true)
        {
            switch (_playerOneStates)
            {
                case PlayerOneStates.PlayerOneIdle:
                    PlayerOneIdle();
                    break;
                case PlayerOneStates.PlayerWalkLeft:
                    PlayerOneWalkLeft();
                    break;
                case PlayerOneStates.PlayerWalkRight:
                    PlayerOneWalkRight();
                    break;
                case PlayerOneStates.PlayerJump:
                    PlayerJump();
                    break;
                case PlayerOneStates.PlayerJumpForward:
                    PlayerOneJumpForward();
                    break;
                case PlayerOneStates.PlayerJumpBackwards:
                    PlayerOneJumpBackwards();
                    break;
                case PlayerOneStates.ComeDownForwards:
                    PlayerOneComeDownForwards();
                    break;
                case PlayerOneStates.ComeDownBackwards:
                    PlayerOneComeDownBackwards();
                    break;
                case PlayerOneStates.ComeDown:
                    PlayerOneComeDown();
                    break;
                case PlayerOneStates.PlayerHighPunch:
                    PlayerHighPunch();
                    break;
                case PlayerOneStates.PlayerLowPunch:
                    PlayerLowPunch();
                    break;
                case PlayerOneStates.PlayerHighKick:
                    PlayerHighKick();
                    break;
                case PlayerOneStates.PlayerLowKick:
                    PlayerLowKick();
                    break;
                case PlayerOneStates.WaitForAnimations:
                    WaitForAnimations();
                    break;
                case PlayerOneStates.PlayerHitByLowKick:
                    PlayerHitByLowKick();
                    break;
                case PlayerOneStates.PlayerHitByHighKick:
                    PlayerHitByHighKick();
                    break;
                case PlayerOneStates.PlayerHitByLeftPunch:
                    PlayerHitByLeftPunch();
                    break;
                case PlayerOneStates.PlayerHitByRightPunch:
                    PlayerHitByRightPunch();
                    break;
                case PlayerOneStates.PlayerDefeated:
                    PlayerDefeated();
                    break;
                case PlayerOneStates.WaitForHitAnimations:
                    WaitForHitAnimations();
                    break;
                case PlayerOneStates.PlayerDemo:
                    PlayerDemo();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return null;
        }
    }

    #region NonSorted

    private void PlayerOneJumpForward()
    {
        //Debug.Log(nameof(PlayerOneJumpForward));

        PlayerOneJumpForwardAnim();

        _playerOneMoveDirection = new Vector3(-_playerJumpHorizontal, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if (_playerOneTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _playerOneStates = PlayerOneStates.ComeDownForwards;
        }
    }

    private void PlayerOneJumpForwardAnim()
    {
        //Debug.Log(nameof(PlayerOneJumpForwardAnim));
    }

    private void PlayerOneJumpBackwards()
    {
        //Debug.Log(nameof(PlayerOneJumpBackwards));

        PlayerOneJumpBackwardsAnim();

        _playerOneMoveDirection = new Vector3(+_playerJumpHorizontal, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);
        
        if (_playerOneTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _playerOneStates = PlayerOneStates.ComeDownBackwards;
        }
    }

    private void PlayerOneJumpBackwardsAnim()
    {
        //Debug.Log(nameof(PlayerOneJumpBackwardsAnim));
    }

    private void PlayerOneComeDownForwards()
    {
        //Debug.Log(nameof(PlayerOneComeDownForwards));

        PlayerOneComeDownForwardsAnim();

        _playerOneMoveDirection = new Vector3(-_playerJumpHorizontal, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (PlayerOneIsGrounded())
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneComeDownForwardsAnim()
    {
        //Debug.Log(nameof(PlayerOneComeDownForwardsAnim));
    }

    private void PlayerOneComeDownBackwards()
    {
        //Debug.Log(nameof(PlayerOneComeDownBackwards));

        PlayerOneComeDownBackwardsAnim();

        _playerOneMoveDirection = new Vector3(+_playerJumpHorizontal, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (PlayerOneIsGrounded())
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneComeDownBackwardsAnim()
    {
        //Debug.Log(nameof(PlayerOneComeDownBackwardsAnim));
    }

    #endregion

    #region HitBy

    private void PlayerHitByRightPunch()
    {
        //Debug.Log(nameof(PlayerHitByRightPunch));
        
        PlayerHitHeadAnimation();

        _playerAudioSource.PlayOneShot(_playerHeadHitAudio);

        Vector3 _impactPoint = global::OpponentPunchRight._playerOneImpactPoint;
        
        var hs = Instantiate(_hitSparks, new Vector3(
                _impactPoint.x,
                _impactPoint.y,
                _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _playerOneStates = PlayerOneStates.WaitForHitAnimations;

    }

    private void PlayerHitByLeftPunch()
    {
        //Debug.Log(nameof(PlayerHitByLeftPunch));
        
        PlayerHitHeadAnimation();

        _playerAudioSource.PlayOneShot(_playerHeadHitAudio);

        Vector3 _impactPoint = global::OpponentPunchLeft._playerOneImpactPoint;
        
        var hs = Instantiate(_hitSparks, new Vector3(
                _impactPoint.x,
                _impactPoint.y,
                _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _playerOneStates = PlayerOneStates.WaitForHitAnimations;

    }

    private void PlayerHitByHighKick()
    {
        //Debug.Log(nameof(PlayerHitByHighKick));
        
        PlayerHitBodyAnimation();

        _playerAudioSource.PlayOneShot(_playerBodyHitAudio);

        Vector3 _impactPoint = global::OpponentHighKick._playerImpactPoint;
        
        var hs = Instantiate(_hitSparks, new Vector3(
                _impactPoint.x,
                _impactPoint.y,
                _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _playerOneStates = PlayerOneStates.WaitForHitAnimations;

    }

    private void PlayerHitByLowKick()
    {
        //Debug.Log(nameof(PlayerHitByLowKick));
        
        PlayerHitBodyAnimation();
        
        _playerAudioSource.PlayOneShot(_playerBodyHitAudio);

        Vector3 _impactPoint = global::OpponentLowKick._playerOneImpactPoint;
        
        var hs = Instantiate(_hitSparks, new Vector3(
            _impactPoint.x,
            _impactPoint.y,
            _impactPoint.z + -.2f),
            Quaternion.identity) as GameObject;

        _playerOneStates = PlayerOneStates.WaitForHitAnimations;
    }

    #endregion

    #region NonSorted2

    private void PlayerDefeated()
    {
        //Debug.Log(nameof(PlayerDefeated));

        _playerOneMoveDirection = new Vector3(0, _playersSpeedYAxis, 0);
        
        PlayerGravityIdle();
        
        if (_playerOneAnimator.IsPlaying(_playerDefeatedFinalHitAnim.name))
        {
            return;
        }
        
        StopCoroutine(PlayerOneFSM());
    }

    private void PlayerGravityIdle()
    {
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);
    }

    private void WaitForHitAnimations()
    {
        //Debug.Log(nameof(WaitForHitAnimations));

        if (_playerOneAnimator.IsPlaying(_playerHitBodyAnim.name))
        {
            return;
        }

        if (_playerOneAnimator.IsPlaying(_playerHitHeadAnim.name))
        {
            return;
        }

        _playerOneStates = PlayerOneStates.PlayerOneIdle;
    }
    
    private void WaitForAnimations()
    {
        //Debug.Log(nameof(WaitForAnimations));

        WaitForAnimationsAnim();

        for (int i = 0; i < _playerAttackAnim.Length; i++)
        {
            if (_playerOneAnimator.IsPlaying(_playerAttackAnim[i].name))
            {
                return;
            }
        }

        _playerIsPunchingLeft = false;
        _playerIsPunchingRight = false;
        _playerIsKickingLow = false;
        _playerIsKickingHigh = false;

        _playerOneStates = PlayerOneStates.PlayerOneIdle;
    }

    private void WaitForAnimationsAnim()
    {
        //Debug.Log(nameof(WaitForAnimationsAnim));
    }

    private void PlayerDemo()
    {
        //Debug.Log(nameof(PlayerDemo));

        PlayerDemoAnim();

        if (Input.GetAxis("LeftTrigger") > .1f)
        {
            transform.Rotate(Vector3.up * _demoRotationValue * Time.deltaTime);
        }

        if (Input.GetAxis("RightTrigger") < -.1f)
        {
            transform.Rotate(Vector3.down * _demoRotationValue * Time.deltaTime);
        }
    }

    private void PlayerDemoAnim()
    {
        //Debug.Log(nameof(PlayerDemoAnim));

        _playerOneAnimator.CrossFade(_playerOneDemoAnim.name);
    }

    private void PlayerOneIdle()
    {
        //Debug.Log(nameof(PlayerOneIdle));

        PlayerOneIdleAnim();

        if (PlayerOneIsGrounded())
        {
            return;
        }

        _playerOneMoveDirection = new Vector3(0, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);
    }

    public void SetPlayerDefeated()
    {
        //Debug.Log(nameof(SetPlayerDefeated));

        PlayerFinalHitAnimation();

        _playerOneStates = PlayerOneStates.PlayerDefeated;
    }

    private void PlayerFinalHitAnimation()
    {
        //Debug.Log(nameof(PlayerFinalHitAnimation));

        _playerOneAnimator.CrossFade(_playerDefeatedFinalHitAnim.name);
    }

    private void PlayerHitBodyAnimation()
    {
        //Debug.Log(nameof(PlayerHitBodyAnimation));
        
        _playerOneAnimator.CrossFade(_playerHitBodyAnim.name);
    }

    private void PlayerHitHeadAnimation()
    {
        //Debug.Log(nameof(PlayerHitHeadAnimation));
        
        _playerOneAnimator.CrossFade(_playerHitHeadAnim.name);
    }

    private void PlayerOneIdleAnim()
    {
        //Debug.Log(nameof(PlayerOneIdleAnim));

        _playerOneAnimator.CrossFade(_playerOneIdleAnim.name);
    }

    #endregion

    #region Walk
    private void PlayerOneWalkLeft()
    {
        //Debug.Log(nameof(PlayerOneWalkLeft));

        PlayerOneRetreatAnim();

        _playerOneMoveDirection = new Vector3(-_playerWalkSpeed, 0, 0);
        MoveDirection();

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);
        if (_analogStickAngle > (0 + _degreeModifier)
            || _analogStickAngle < 0 - _degreeModifier)
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void MoveDirection()
    {
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection).normalized;
        _playerOneMoveDirection *= _playerWalkSpeed;
    }

    private void PlayerOneWalkRight()
    {
        //Debug.Log(nameof(PlayerOneWalkRight));

        PlayerOneWalkAnim();

        _playerOneMoveDirection = new Vector3(+_playerWalkSpeed, 0, 0);
        MoveDirection();

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);


        if (_analogStickAngle > (0 + _degreeModifier)
                || _analogStickAngle < 0 - _degreeModifier)
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneWalkAnim()
    {
        //Debug.Log(nameof(PlayerOneWalkAnim));

        _playerOneAnimator.CrossFade(_playerOneWalkAnim.name);

        if (Math.Abs(_playerOneAnimator.State(_playerOneWalkAnim.name).speed - _playerWalkSpeed) < 0.01f)
        {
            return;
        }

        if (_playerOneAnimator.State(_playerOneWalkAnim.name).speed < _playerWalkSpeed)
        {
            _playerOneAnimator.GetAnimator(_playerOneAnimator.name).speed = _playerWalkSpeed;
        }
    }
    #endregion

    #region Jump
    private void PlayerJump()
    {
        //Debug.Log(nameof(PlayerJump));

        PlayerOneJumpAnim();

        _playerOneMoveDirection = new Vector3(0, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection).normalized;
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);
        
        if (_playerOneTransform.transform.position.y >= _jumpHeightTemp.y)
        {
            _playerOneStates = PlayerOneStates.ComeDown;
        }
    }

    private void PlayerOneComeDown()
    {
        //Debug.Log(nameof(PlayerOneComeDown));

        PlayerOneComeDownAnim();

        _playerOneMoveDirection = new Vector3(0, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        // SetIdleToState();
        if (PlayerOneIsGrounded())
        {
            _playerOneStates = PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerOneJumpAnim()
    {
        //Debug.Log(nameof(PlayerOneJumpAnim));

        _playerOneAnimator.CrossFade(_playerOneJumpAnim.name);
    }
    #endregion

    #region Punch
    private void PlayerHighPunchAnim()
    {
        //Debug.Log(nameof(PlayerHighPunchAnim));

        _playerOneAnimator.CrossFade(_playerAttackAnim[0].name);
    }

    private void PlayerLowPunchAnim()
    {
        //Debug.Log(nameof(PlayerLowPunchAnim));

        _playerOneAnimator.CrossFade(_playerAttackAnim[1].name);
    }
    #endregion

    #region Kick
    private void PlayerLowKickAnim()
    {
        //Debug.Log(nameof(PlayerLowKickAnim));

        _playerOneAnimator.CrossFade(_playerAttackAnim[2].name);
    }

    private void PlayerHighKickAnim()
    {
        //Debug.Log(nameof(PlayerHighKickAnim));

        _playerOneAnimator.CrossFade(_playerAttackAnim[3].name);
    }
    #endregion

    #region Anim

    private void PlayerOneRetreatAnim()
    {
        //Debug.Log(nameof(PlayerOneRetreatAnim));

        _playerOneAnimator.CrossFade(_playerOneWalkAnim.name);

        if (Math.Abs(_playerOneAnimator.State(_playerOneWalkAnim.name).speed - _playerRetreatSpeed) < 0.01f)
        {
            return;
        }

        if (_playerOneAnimator.State(_playerOneWalkAnim.name).speed > _playerRetreatSpeed)
        {
            _playerOneAnimator.GetAnimator(_playerOneAnimator.name).speed = _playerRetreatSpeed;
        }
    }

    private void PlayerHighPunch()
    {
        //Debug.Log(nameof(PlayerHighPunch));

        PlayerHighPunchAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    private void PlayerLowPunch()
    {
        //Debug.Log(nameof(PlayerLowPunch));

        PlayerLowPunchAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    private void PlayerHighKick()
    {
        //Debug.Log(nameof(PlayerHighKick));

        PlayerHighKickAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    private void PlayerLowKick()
    {
        //Debug.Log(nameof(PlayerLowKick));

        PlayerLowKickAnim();

        _playerOneStates = PlayerOneStates.WaitForAnimations;
    }

    #endregion

    #region Managers

    private void AttackInputManager()
    {
        //Debug.Log(nameof(AttackInputManager));

        if (Input.GetButtonDown("Fire1"))
        {
            _playerOneStates = PlayerOneStates.PlayerLowKick;

            _playerIsKickingLow = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _playerOneStates = PlayerOneStates.PlayerHighKick;
            _playerIsKickingHigh = true;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            _playerOneStates = PlayerOneStates.PlayerHighPunch;

            _playerIsPunchingLeft = true;
        }

        if (Input.GetButtonDown("Fire4"))
        {
            _playerOneStates = PlayerOneStates.PlayerLowPunch;

            _playerIsPunchingRight = true;
        }
    }

    private void PlayerInputController()
    {
        //Debug.Log(nameof(PlayerInputController));

        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");

        _analogStickAngle =
            Mathf.Atan2
                (_yAxis, _xAxis)
                * Mathf.Rad2Deg;
    }

    private void HorizontalJumpInputManager()
    {
        //Debug.Log(nameof(HorizontalJumpInputManager));

        if (Input.GetAxis("Vertical") > _controllerDeadZonePos && Input.GetAxis("Horizontal") > _controllerDeadZoneNeg)
        {
            if (_analogStickAngle > 45 + _degreeModifier
                || _analogStickAngle < 45 - _degreeModifier)
            {
                return;
            }
            else
            {
                _playerOneStates = PlayerOneStates.PlayerJumpForward;
            }
        }
        
        if (Input.GetAxis("Vertical") > _controllerDeadZonePos 
            || Input.GetAxis("Horizontal") < _controllerDeadZoneNeg)
        {
            if (_analogStickAngle > 135 + _degreeModifier
                || _analogStickAngle < 135 - _degreeModifier)
            {
                return;
            }
            else
            {
                _playerOneStates = PlayerOneStates.PlayerJumpBackwards;
            }
        }
    }

    private void HorizontalInputManagerAnim()
    {
        //Debug.Log(nameof(HorizontalInputManagerAnim));
    }

    private void StandardInputManager()
    {
        //Debug.Log(nameof(StandardInputManager));

        if (Input.GetAxis("Horizontal") < _controllerDeadZoneNeg)
        {
            if (_analogStickAngle < (_180DegreesAngle - _degreeModifier)
                && _analogStickAngle > 0 - (_180DegreesAngle + _degreeModifier))
            {
                return;
            }
            else
            {
                _playerOneStates = PlayerOneStates.PlayerWalkLeft;
            }
        }

        if (Input.GetAxis("Horizontal") > _controllerDeadZonePos)
        {
            if ((_analogStickAngle > (0 + _degreeModifier)
                 || _analogStickAngle < 0 - _degreeModifier)
                || Input.GetAxis("Horizontal") == 0)
            {
                return;
            }
            else
            {
                _playerOneStates = PlayerOneStates.PlayerWalkRight;
            }
        }

        if (Input.GetAxis("Vertical") > _controllerDeadZonePos)
        {
            if (_analogStickAngle > 90 + _degreeModifier
            || _analogStickAngle < 90 - _degreeModifier)
            {
                return;
            }
            else
            {
                _playerOneStates = PlayerOneStates.PlayerJump;
            }
        }
    }
    #endregion

    #region InputAnim

    private void StandardInputManagerAnim()
    {
        //Debug.Log(nameof(StandardInputManagerAnim));
    }

    private void AttackInputManagerAnim()
    {
        //Debug.Log(nameof(AttackInputManagerAnim));
    }

    #endregion
    
    private void UpdatePlayersPlanePosition()
    {
        //Debug.Log(nameof(UpdatePlayersPlanePosition));

        var TOLERANCE = 0.01f;
        if (Math.Abs(_playerController.transform.position.z - GameManager._playerStartingPosition.z) > TOLERANCE)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                GameManager._opponentsStartingPosition.z);
        }
    }

    private void ApplyGravity()
    {
        //Debug.Log(nameof(ApplyGravity));

        if (PlayerOneIsGrounded())
        {
            _playersSpeedYAxis = 0f;
        }
        else
        {
            _playersSpeedYAxis -= _playersGravity * _playerGravityModifier * Time.deltaTime;
        }
    }

    private bool PlayerOneIsGrounded()
    {
        Debug.Log($"{nameof(PlayerOneIsGrounded)} - {(_collisionFlags & CollisionFlags.Below) != 0}");
        //return (_collisionFlags & CollisionFlags.Below) != 0;
        return true;
    }
    
    #region UpdateRotation
    private void UpdatePlayerPosition()
    {
        //Debug.Log(nameof(UpdatePlayerPosition));

        _playersPosition = new Vector3(_playerOne.transform.position.x, _playerOne.transform.position.y, _playerOne.transform.position.z);
    }

    private void UpdateOpponentsPosition()
    {
        //Debug.Log(nameof(UpdateOpponentsPosition));

        _opponentPosition = new Vector3(_opponent.transform.position.x, _opponent.transform.position.y, _opponent.transform.position.z);
    }

    private void UpdatePlayersRotation()
    {
        //Debug.Log(nameof(UpdatePlayersRotation));

        if (_playerOne.transform.position.x < _opponent.transform.position.x)
        {
            if (Math.Abs(_playerOne.transform.rotation.y - _defaultRotation) < 0.001)
            {
                return;
            }
            else
            {
                _targetRotation = Quaternion.Euler(0, _defaultRotation, 0);
                
                _playerOne.transform.rotation = 
                    Quaternion.Slerp(
                        transform.rotation, 
                        _targetRotation, 
                        Time.deltaTime * _rotationSpeed);
            }
        }
        
        if (_playerOne.transform.position.x > _opponent.transform.position.x)
        {
            
            if (Math.Abs(_playerOne.transform.rotation.y - _alternativeRotation) < 0.001)
            {
                return;
            }
            else
            {
                _targetRotation = Quaternion.Euler(0, _alternativeRotation, 0);
                
                _playerOne.transform.rotation = 
                    Quaternion.Slerp(
                        transform.rotation, 
                        _targetRotation, 
                        Time.deltaTime * _rotationSpeed);
                
                Debug.LogWarning("TEST");
            }
        }
    }

    #endregion

    private void PlayerOneComeDownAnim()
    {
        //Debug.Log(nameof(PlayerOneComeDownAnim));
    }
}