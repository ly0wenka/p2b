using System;

[System.Serializable]
public class ChallengeModeOptions : ICloneable {
    public string challengeName = "";
    public string description = "";
    public CharacterInfo character;
    public CharacterInfo opCharacter;
    public SimpleAIBehaviour ai;
    public bool isCombo;
    public bool aiOpponent;
    public bool resetData;
    public int repeats = 1;
    public ChallengeAutoSequence challengeSequence;
    public bool actionListToggle;
    public ActionSequence[] actionSequence = new ActionSequence[0];

    public object Clone() {
        return CloneObject.Clone(this);
    }
}