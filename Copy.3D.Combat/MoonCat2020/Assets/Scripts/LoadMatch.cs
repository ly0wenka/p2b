using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadMatch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SendMessageSceneBackgroundLoadFromOnePlayerManager();
        SendMessageSceneBackgroundLoadFromOpponentManager();
    }

    private void SendMessageSceneBackgroundLoadFromOnePlayerManager()
    {
        var onePlayerManager = GameObject.FindGameObjectWithTag(nameof(OnePlayerManager));

        #region InstatiateWhenNull
        if (!onePlayerManager)
        {
            Instantiate(Resources.Load($"Managers{Path.PathSeparator}OnePlayerManager"));
        }
        #endregion
        
        onePlayerManager.GetComponent<OnePlayerManager>()
        .SendMessage(nameof(OnePlayerManager.LoadPlayerOneCharacter));
    }

    private void SendMessageSceneBackgroundLoadFromOpponentManager()
    {
        var opponentManager = GameObject.FindGameObjectWithTag(nameof(OpponentManager));

        #region InstatiateWhenNull
        if (!opponentManager)
        {
            Instantiate(Resources.Load($"Managers{Path.PathSeparator}OnePlayerManager"));
        }
        #endregion
        
        opponentManager.GetComponent<OpponentManager>()
        .SendMessage(nameof(OpponentManager.LoadCurrentOpponent));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
