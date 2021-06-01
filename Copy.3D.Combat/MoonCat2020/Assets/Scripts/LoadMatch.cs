using System.Collections;
using System.Collections.Generic;
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
        GameObject.FindGameObjectWithTag(nameof(OnePlayerManager))
            .GetComponent<OnePlayerManager>()
            .SendMessage("LoadPlayerOneCharacter");
    }

    private void SendMessageSceneBackgroundLoadFromOpponentManager()
    {
        GameObject.FindGameObjectWithTag(nameof(OpponentManager))
            .GetComponent<OpponentManager>()
            .SendMessage("LoadCurrentOpponent");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
