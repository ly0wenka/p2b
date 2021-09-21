using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class VisitorTest
{
    [Test]
    public void _0_VisitLight()
    {
        IVisitor bullet = new TankBullet();
        ILightUnit light = new Marine();

        light.Accept(bullet);

        Assert.AreEqual(100 - 21, light.Health);
    }

    [Test]
    public void _1_VisitArmored()
    {
        IVisitor bullet = new TankBullet();
        IArmoredUnit armored = new Marauder();

        armored.Accept(bullet);

        Assert.AreEqual(125 - 32, armored.Health);
    }
    // A Test behaves as an ordinary method
    [Test]
    public void VisitorTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator VisitorTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
