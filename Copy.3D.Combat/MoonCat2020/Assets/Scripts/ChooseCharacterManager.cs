using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterManager : MonoBehaviour
{
    public static bool _robotBlack;
    public static bool _robotWhite;
    public static bool _robotRed;
    public static bool _robotBlue;
    public static bool _robotBrown;
    public static bool _robotGreen;
    public static bool _robotPink;
    public static bool _robotGold;

    void Awake()
    {
        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
        _robotBrown = false;
        _robotGreen = false;
        _robotPink = false;
        _robotGold = false;
    }
    
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
