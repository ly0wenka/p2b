using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBarVisibility : MonoBehaviour
{
    public Transform controlBar;
    // Start is called before the first frame update
    void Start()
    {
        controlBar.gameObject.SetActive(Application.platform == RuntimePlatform.Android);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
