using NUnit.Framework;
using Tests.Utils;
using Tests.Utils.Arranges;

namespace Tests.EditMode
{
    public class PlayerOneMovementTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void DefaultWalkSpeed()
        {
            var playerOneMovement = Create.Component<PlayerOneMovement>();;
            Assert.AreEqual(1f, playerOneMovement._playerWalkSpeed);
        }
    }
}
