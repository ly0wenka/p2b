using System.Collections.Generic;

[System.Serializable]
public class StoryMode{
    public bool useSameStoryForAllCharacters;
    public bool canCharactersFightAgainstThemselves;
    public CharacterStory defaultStory;

    public List<int> selectableCharactersInStoryMode = new List<int>();
    public List<int> selectableCharactersInVersusMode = new List<int>();
    public CharacterStories characterStories = new CharacterStories();
}