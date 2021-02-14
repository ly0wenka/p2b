using System.Collections.Generic;

[System.Serializable]
public class StoryModeBattle{
    public StoryModeScreen conversationBeforeBattle;
    public StoryModeScreen conversationAfterBattle;
    public int opponentCharacterIndex;
    public List<int> possibleStagesIndexes = new List<int>();
}