using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void DefaultWalkSpeed()
    {
        var gameObject = new GameObject();
        var playerOneMovement = gameObject.AddComponent<PlayerOneMovement>();
        Assert.AreEqual(1f, playerOneMovement._playerWalkSpeed);
    }
}
