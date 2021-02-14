using System.Collections.Generic;

public class StoryModeInfo
{
    // The information about the character story
    public CharacterStory characterStory = null;

    // Whether the character can fight against himself in Story Mode
    public bool canFightAgainstHimself = false;

    // The index of the current "group"
    public int currentGroup = 0;

    // The index of the current "battle" in the current "group"
    public int currentBattle = 0;

    // The information about the current battle
    public StoryModeBattle currentBattleInformation = null;

    // The indexes of the characters that have been defeated in the current "group".
    // It's used only if the player must fight the opponents in a group in random order.
    public HashSet<int> defeatedOpponents = new HashSet<int>();
}