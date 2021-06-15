using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloScript : MonoBehaviour
{
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (count % 60 == 0)
        {
            Debug.Log(string.Format("Time has come set to: {0}", count / 60));
        }
        count++;
    }
}
