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
        
        InstatiateWhenNull(onePlayerManager);
        
        onePlayerManager.GetComponent<OnePlayerManager>()
        .SendMessage(nameof(OnePlayerManager.LoadPlayerOneCharacter));
    }

    private static void InstatiateWhenNull(GameObject onePlayerManager)
    {
        if (!onePlayerManager)
        {
            Instantiate(Resources.Load($"Managers{Path.PathSeparator}{nameof(OnePlayerManager)}"));
        }
    }

    private void SendMessageSceneBackgroundLoadFromOpponentManager()
    {
        var opponentManager = GameObject.FindGameObjectWithTag(nameof(OpponentManager));

        #region InstatiateWhenNull
        if (!opponentManager)
        {
            Instantiate(Resources.Load($"Managers{Path.PathSeparator}{nameof(OpponentManager)}"));
        }
        #endregion
        
        opponentManager.GetComponent<OpponentManager>()
        .SendMessage(nameof(OpponentManager.LoadCurrentOpponent));
    }
}
