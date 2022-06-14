namespace Tests.Utils.Arranges
{
    public static class Setup
    {
        public static MainMenu MainMenu(int selectedButton = 0)
        {
            var mainMenu = Find.MainMenu();
            //mainMenu.SelectedButton = selectedButton;
            return mainMenu;
        }
    }
}