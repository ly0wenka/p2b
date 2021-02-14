[System.Serializable]
public class CharacterStory{
    public StoryModeScreen opening;
    public StoryModeScreen ending;
    public FightsGroup[] fightsGroups = new FightsGroup[0];
    public bool showStoryInEditor;
}