using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int _playersStartingYRot = 180;

    public float _playerStartingPosXAxis = -1f;
    public float _playerStartingPosYAxis = 0f;
    public float _playerStartingPosZAxis = -7f;

    public float _opponentStartingPosXAxis = 1f;
    public float _opponentStartingPosYAxis = 0f;
    public float _opponentStartingPosZAxis = -7f;
    
    public static Vector3 _playerStartingPosition;
    public static Vector3 _playerStartingRotation = new Vector3(0, _playersStartingYRot, 0);
    
    private static int _opponentsStartingYRot = 0;
    public static Vector3 _opponentsStartingPosition;
    public static Vector3 _opponentsStartingRotation = new Vector3(0, _opponentsStartingYRot, 0);
    
    void Awake()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        DisableOnePlayerManager();
        DisableTwoPlayerManager();

        _playerStartingPosition = new Vector3(
            _playerStartingPosXAxis,
            _playerStartingPosYAxis,
            _playerStartingPosZAxis);

        _playerStartingPosition = new Vector3(
            _opponentStartingPosXAxis,
            _opponentStartingPosYAxis,
            _opponentStartingPosZAxis);
    }

    private static void DisableTwoPlayerManager() 
        => GameObject.FindGameObjectWithTag("TwoPlayerManager").GetComponent<TwoPlayerManager>().enabled = false;

    private static void DisableOnePlayerManager() 
        => GameObject.FindGameObjectWithTag("OnePlayerManager").GetComponent<OnePlayerManager>().enabled = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
