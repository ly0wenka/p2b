public class PauseScreen : CombatScreen
{
    public virtual void GoToMainMenu()
    {
        MainScript.PauseGame(false);
        MainScript.StartMainMenuScreen();
    }

    public virtual void ResumeGame() => MainScript.PauseGame(false);
}