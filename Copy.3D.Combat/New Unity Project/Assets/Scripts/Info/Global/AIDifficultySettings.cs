﻿using System;

[System.Serializable]
public class AIDifficultySettings : ICloneable
{
    public AIDifficultyLevel difficultyLevel;
    public bool overrideTimeBetweenDecisions;
    public float timeBetweenDecisions = 0;
    public bool overrideTimeBetweenActions;
    public float timeBetweenActions = 0.05f;
    public bool overrideAggressiveness;
    public float aggressiveness = 0.5f;
    public bool overrideRuleCompliance;
    public float ruleCompliance = .9f;
    public bool overrideComboEfficiency;
    public float comboEfficiency = 1f;
    public AIBehaviour startupBehavior;

    public object Clone() => CloneObject.Clone(this);
}