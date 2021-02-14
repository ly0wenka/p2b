public class StoryModeContinueScreen : CombatScreen
{
    public virtual void RepeatBattle() => MainScript.StartStoryModeBattle();

    public virtual void GoToGameOverScreen() => MainScript.StartStoryModeGameOverScreen();
}