using System.Net;

public class NetworkGameScreen : CombatScreen
{
    public virtual void GoToMainMenu() => MainScript.StartMainMenuScreen();

    public virtual void GoToHostGameScreen() => MainScript.StartHostGameScreen();

    public virtual void GoToJoinGameScreen() => MainScript.StartJoinGameScreen();

    public virtual string GetIp()
    {
        string hostName = System.Net.Dns.GetHostName();
        IPHostEntry ipHostEntry = System.Net.Dns.GetHostEntry(hostName);
        IPAddress[] ipAddresses = ipHostEntry.AddressList;

        return ipAddresses[ipAddresses.Length - 1].ToString();
    }
}