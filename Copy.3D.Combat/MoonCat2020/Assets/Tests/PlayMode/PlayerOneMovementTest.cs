using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class PlayerOneMovementTest
    {
        [UnityTest]
        public IEnumerator PlayerOneMovement_playerIsPunchingLeftShouldFalse()
        {
            yield return null;
            Assert.AreEqual(false, PlayerOneMovement._playerIsPunchingLeft);
        }
    }
}
