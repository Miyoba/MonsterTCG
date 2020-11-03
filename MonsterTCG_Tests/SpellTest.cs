using System;
using System.Collections.Generic;
using MonsterTCG;
using NUnit.Framework;
using Moq;


namespace MonsterTCG_Tests
{
    class SpellTest
    {
        [Test]
        public void TestSpell_CheckDamageWaterSpellVsWaterSpell()
        {
            // arrange
            var spellA = new SpellCard("BubblePrison", 3, EnumElementType.Water);
            var spellB = new SpellCard("Tsunami", 6, EnumElementType.Water);

            // act
            var damageA = spellA.GetDamage(spellB);
            var damageB = spellB.GetDamage(spellA);


            // assert
            Assert.AreEqual(3,damageA);
            Assert.AreEqual(6,damageB);
        }
    }
}
