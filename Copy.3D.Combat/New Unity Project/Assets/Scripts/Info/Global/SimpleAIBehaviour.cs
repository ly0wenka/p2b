using System;
using UnityEngine;

[System.Serializable]
public class SimpleAIBehaviour : ScriptableObject{
    public SimpleAIStep[] steps = new SimpleAIStep[0];
    public bool blockAfterFirstHit;

    [HideInInspector]
    public bool showInInspector;

    [HideInInspector]
    public bool showStepsInInspector;

}