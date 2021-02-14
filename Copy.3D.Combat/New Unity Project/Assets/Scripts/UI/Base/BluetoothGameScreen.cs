public class BluetoothGameScreen : CombatScreen
{
    public virtual void GoToMainMenu() => MainScript.StartMainMenuScreen();

    public virtual void HostGame() => MainScript.HostBluetoothGame();

    public virtual void JoinGame() => MainScript.JoinBluetoothGame();
}