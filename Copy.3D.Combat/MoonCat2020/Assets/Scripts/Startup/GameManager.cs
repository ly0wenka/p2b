using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int _playersStartingYRot = 180;
    public static Vector3 _playerStartingPosition = new Vector3(-1, 0, -7);
    public static Vector3 _playerStartingRotation = new Vector3(0, _playersStartingYRot, 0);
    
    private static int _opponentsStartingYRot = 0;
    public static Vector3 _opponentsStartingPosition = new Vector3(1, 0, -7);
    public static Vector3 _opponentsStartingRotation = new Vector3(0, _opponentsStartingYRot, 0);
    
    void Awake()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        DisableOnePlayerManager();
        DisableTwoPlayerManager();
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
