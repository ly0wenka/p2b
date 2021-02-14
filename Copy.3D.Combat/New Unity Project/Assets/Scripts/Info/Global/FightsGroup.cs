[System.Serializable]
public class FightsGroup{
    public int maxFights = 4; // maxFights is only used when mode == FightsGroupMode.FightAgainstSeveralRandomOpponents
    public FightsGroupMode mode = FightsGroupMode.FightAgainstAllOpponentsInTheGroupInRandomOrder;
    public string name = string.Empty;
    public StoryModeBattle[] opponents = new StoryModeBattle[0];
    public bool showOpponentsInEditor;
}