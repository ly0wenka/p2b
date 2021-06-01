using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundManager : MonoBehaviour
{
    public string _selectedBackground = "";
    public int _backgroundCounter;

    public string[] _backgroundScenes = new string[]
    {
        "Scene0",
        "Scene1",
        "Scene2",
        "Scene3",
        "Scene4",
        "Scene5",
        "Scene6",
        "Scene7",
    };
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        ResetBackgroundCounter();
    }

    private void ResetBackgroundCounter()
    {
        _backgroundCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SceneBackgroundManager()
    {
        Debug.Log(nameof(SceneBackgroundManager));

        if (_backgroundCounter < _backgroundScenes.Length)
        {
            _backgroundCounter++;
        }

        if (_backgroundCounter == _backgroundScenes.Length)
        {
            ResetBackgroundCounter();
        }

        _selectedBackground = _backgroundScenes[_backgroundCounter];
        


    }

    private void SceneBackgroundLoad()
    {
        Debug.Log(nameof(SceneBackgroundLoad));
        
        SceneBackgroundManager();
        
        SceneManager.LoadScene(_selectedBackground);
    }
}
