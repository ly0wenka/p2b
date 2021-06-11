using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        DisableOnePlayerManager();
        DisableTwoPlayerManager();
    }

    private static void DisableTwoPlayerManager()
    {
        GameObject.FindGameObjectWithTag("TwoPlayerManager").GetComponent<TwoPlayerManager>().enabled = false;
    }

    private static void DisableOnePlayerManager()
    {
        GameObject.FindGameObjectWithTag("OnePlayerManager").GetComponent<OnePlayerManager>().enabled = false;
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
