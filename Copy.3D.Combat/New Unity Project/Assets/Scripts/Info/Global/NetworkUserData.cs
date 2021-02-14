using System;

[System.Serializable]
public class NetworkUserData : ICloneable
{
    public string variableName;
    public ServerVariableType variableType;
    public float floatValue;
    public int intValue;
    public string stringValue;
    public bool boolValue;
    public ServerVariableUpdateType variableUpdateType;
    public MatchMakingFilterType matchMakingFilterType;

    public CombatBoolean combatBoolean;

    public object Clone() => CloneObject.Clone(this);
}