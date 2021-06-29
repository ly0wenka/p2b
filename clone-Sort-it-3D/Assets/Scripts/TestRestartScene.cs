using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestRestartScene : MonoBehaviour
{
    //public GameObject Loading;
    public void OnMouseUpAsButton()
    { 
        
            //Loading.SetActive(true);
            
        SceneManager.LoadScene(0);       
    }
}
