using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerOneMovementTest
{
    [UnityTest]
    public IEnumerator PlayerIsNotPunchingLeft()
    {
        var gameObject = new GameObject();
        var playerOneMovement = gameObject.AddComponent<PlayerOneMovement>();
        
        yield return null; // wait until animations
        Assert.AreEqual(false, PlayerOneMovement._playerIsPunchingLeft);
    }
}
