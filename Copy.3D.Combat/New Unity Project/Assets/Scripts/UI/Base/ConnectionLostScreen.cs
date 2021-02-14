public class ConnectionLostScreen : CombatScreen
{
    public virtual void GoToMainMenu() => MainScript.StartMainMenuScreen();

    public virtual void GoToNetworkGameScreen() => MainScript.GoToNetworkGameScreen();
}