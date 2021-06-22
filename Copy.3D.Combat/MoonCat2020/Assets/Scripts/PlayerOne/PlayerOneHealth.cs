using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneHealth : MonoBehaviour
{
    public static int _minimumPlayerHealth = 0;
    public static int _maximumPlayerHealth = 100;
    public static int _currentPlayerHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        _currentPlayerHealth = _maximumPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
