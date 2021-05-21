using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    public int _selectedButton = 0;
    public float _timeBetweenButtonPress = 0.1f;
    public float _timeDelay;

    public float _mainMenuVerticalInputTimer;
    public float _mainMenuVerticalInputDelay = 1f;
    
    
    
    private bool _ps4Controller;
    private bool _xBOXController;
    private string[] _mainMenuButtons = new string[]
    {
        "_onePlayer",
        "_twoPlayer",
        "_quit"
    };

    private MainMenuController _mainMenuController;
    // Start is called before the first frame update
    void Start()
    {
        _ps4Controller = false;
        _xBOXController = false;
        _mainMenuController = MainMenuController.MainMenuFadeIn;
        
        StartCoroutine(MainMenuManager());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MainMenuManager()
    {
        while (true)
        {
            switch (_mainMenuController)
            {
                case MainMenuController.MainMenuFadeIn:
                    MainMenuFadeIn();
                    break;
                case MainMenuController.MainMenuAtIdle:
                    MainMenuAtIdle();
                    break;
                case MainMenuController.MainMenuFadeOut:
                    MainMenuFadeOut();
                    break;
            }

            yield return null;
        }
    }

    private void MainMenuFadeIn()
    {
        Debug.Log(nameof(MainMenuFadeIn));
    }

    private void MainMenuAtIdle()
    {
        Debug.Log(nameof(MainMenuAtIdle));
    }

    private void MainMenuFadeOut()
    {
        Debug.Log(nameof(MainMenuFadeOut));
    }

    private void MainMenuButtonPress()
    {
        Debug.Log(nameof(MainMenuFadeIn));
    }
}
