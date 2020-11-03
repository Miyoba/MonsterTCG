using System;
using System.Collections.Generic;
using MonsterTCG;
using NUnit.Framework;
using Moq;


namespace MonsterTCG_Tests
{
    class MonsterTest
    {
        [Test]
        public void TestMonster_CheckDamageDragonVsDragon()
        {
            // arrange
            var dragonA = new Dragon("Smaug", 10, EnumElementType.Fire);
            var dragonB = new Dragon("Toothless", 5, EnumElementType.Normal);

            // act
            var damageA = dragonA.GetDamage(dragonB);
            var damageB = dragonB.GetDamage(dragonA);


            // assert
            Assert.AreEqual(10,damageA);
            Assert.AreEqual(5,damageB);
        }
    }
}
