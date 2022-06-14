using Startup;

namespace Tests.Utils.Arranges
{
    public static class Setup
    {
        public static MainMenu MainMenu(int selectedButton = 0)
        {
            var mainMenu = Find.MainMenu();
            mainMenu.selectedButton = selectedButton;
            return mainMenu;
        }
    }
}