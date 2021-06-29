using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCamera : MonoBehaviour
{
    private Vector3 _cameraStartingPosition = new Vector3(0, 1, -10);

    private GameObject _fightCamera;

    private float _cameraValueXAxis;
    private float _cameraValueZAxis;

    private int _cameraValueZAxisModifier = -8;

    public static GameObject _playerOne;
    public static GameObject _opponent;

    private Vector3 _playersPosition;
    private Vector3 _opponentPosition;
    // Start is called before the first frame update
    void Start()
    {
        _fightCamera = GameObject.FindGameObjectWithTag("MainCamera");

        _fightCamera.transform.position = _cameraStartingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPosition();
        UpdateOpponentsPosition();
        UpdateCameraPosition();
    }

    private void UpdatePlayerPosition()
    {
        Debug.Log(nameof(UpdatePlayerPosition));

        _playersPosition = new Vector3(_playerOne.transform.position.x, _playerOne.transform.position.y, _playerOne.transform.position.z);
    }

    private void UpdateOpponentsPosition()
    {
        Debug.Log(nameof(UpdateOpponentsPosition));

        _opponentPosition = new Vector3(_opponent.transform.position.x, _opponent.transform.position.y, _opponent.transform.position.z);
    }

    private void UpdateCameraPosition()
    {
        Debug.Log(nameof(UpdateCameraPosition));

        _cameraValueXAxis = (_playerOne.transform.position.x + _opponent.transform.position.x) / 2;

        if (_playerOne.transform.position.x < _opponent.transform.position.x)
        {
            _cameraValueZAxis = _playerOne.transform.position.x + _opponent.transform.position.x;
        }
        
        if (_playerOne.transform.position.x > _opponent.transform.position.x)
        {
            _cameraValueZAxis = _opponent.transform.position.x - _playerOne.transform.position.x ;
        }

        if (_cameraValueZAxis > -2)
        {
            _cameraValueZAxis = -2;
        }

        _fightCamera.transform.position =
            new Vector3(_cameraValueXAxis, 1, _cameraValueZAxis + _cameraValueZAxisModifier);
    }
}
