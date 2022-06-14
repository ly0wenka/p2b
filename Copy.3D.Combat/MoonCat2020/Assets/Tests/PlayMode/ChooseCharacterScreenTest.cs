using System.Collections;
using Startup;
using Tests.Utils;
using Tests.Utils.Arranges;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class ChooseCharacterScreenTest
    {
        /// <summary>
        /// 1. Load Splash Screen
        /// 2. Press 0 Button at Main Menu Scene
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator WhenLoadSceneSplashScreen()
        {
            yield return Load.SceneAsync("Splash Screen"); // wait until animations
            //Assert.AreEqual(false, PlayerOneMovement._playerIsPunchingLeft);
            yield return Wait.For3Seconds;
            var mainMenu = Setup.MainMenu();
            mainMenu.MainMenuButtonPress();
            yield return Wait.For100MilliSeconds;
      //      Assert.IsNotNull(button);
            yield return Wait.For20Seconds;
        }
    }
}