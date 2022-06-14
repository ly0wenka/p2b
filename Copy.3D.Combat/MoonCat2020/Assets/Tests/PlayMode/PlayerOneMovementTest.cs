using System.Collections;
using NUnit.Framework;
using Tests.Utils;
using Tests.Utils.Arranges;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class PlayerOneMovementTest
    {
        [UnityTest]
        public IEnumerator PlayerIsNotPunchingLeft()
        {
            var playerOneMovement = Create.Component<PlayerOneMovement>();
        
            yield return null; // wait until animations
            Assert.AreEqual(false, PlayerOneMovement._playerIsPunchingLeft);
        }
    }
}
