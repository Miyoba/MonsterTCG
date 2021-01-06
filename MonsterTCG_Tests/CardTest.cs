using MonsterTCG;
using NUnit.Framework;


namespace MonsterTCG_Tests
{
    class CardTest
    {
        [Test]
        public void TestMonster_CheckDamageMonsterElementTest_BaseDamage()
        {
            // arrange
            var dragonA = new Dragon("Smaug", 10, EnumElementType.Fire);
            var dragonB = new Dragon("Toothless", 5, EnumElementType.Normal);
            var dragonC = new Dragon("Nessie", 1, EnumElementType.Water);

            // act
            var damageAb = dragonA.GetDamage(dragonB);
            var damageAc = dragonA.GetDamage(dragonC);
            var damageBa = dragonB.GetDamage(dragonA);
            var damageBc = dragonB.GetDamage(dragonC);
            var damageCa = dragonC.GetDamage(dragonA);
            var damageCb = dragonC.GetDamage(dragonB);

            // assert
            Assert.AreEqual(10,damageAb);
            Assert.AreEqual(damageAb, damageAc);
            Assert.AreEqual(5,damageBa);
            Assert.AreEqual(damageBa,damageBc);
            Assert.AreEqual(1,damageCa);
            Assert.AreEqual(damageCa,damageCb);
        }

        [Test]
        public void TestMonster_CheckDamageDragonVsDragon_BaseDamage()
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

        [Test]
        public void TestMonster_CheckDamageGoblinVsDragon_GoblinNoDamage()
        {
            // arrange
            var goblinA = new Goblin("Skip", 10, EnumElementType.Water);
            var dragonB = new Dragon("Toothless", 5, EnumElementType.Normal);

            // act
            var damageA = goblinA.GetDamage(dragonB);
            var damageB = dragonB.GetDamage(goblinA);

            // assert
            Assert.AreEqual(0,damageA);
            Assert.AreEqual(5,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageGoblinVsGoblin_BaseDamage()
        {
            // arrange
            var goblinA = new Goblin("Skip", 10, EnumElementType.Water);
            var goblinB = new Goblin("Trip", 5, EnumElementType.Normal);

            // act
            var damageA = goblinA.GetDamage(goblinB);
            var damageB = goblinB.GetDamage(goblinA);

            // assert
            Assert.AreEqual(10,damageA);
            Assert.AreEqual(5,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageDragonVsFireelf_DragonNoDamage()
        {
            // arrange
            var dragonA = new Dragon("Smaug", 10, EnumElementType.Fire);
            var fireelfB = new Fireelf("Cinthiael Woodrunner", 5, EnumElementType.Normal);

            // act
            var damageA = dragonA.GetDamage(fireelfB);
            var damageB = fireelfB.GetDamage(dragonA);


            // assert
            Assert.AreEqual(0,damageA);
            Assert.AreEqual(5,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageFireelfVsFireelf_BaseDamage()
        {
            // arrange
            var fireelfA = new Fireelf("Fedrian Waterseer", 10, EnumElementType.Water);
            var fireelfB = new Fireelf("Cinthiael Woodrunner", 5, EnumElementType.Normal);

            // act
            var damageA = fireelfA.GetDamage(fireelfB);
            var damageB = fireelfB.GetDamage(fireelfA);

            // assert
            Assert.AreEqual(10,damageA);
            Assert.AreEqual(5,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageWizardVsOrc_OrcNoDamage()
        {
            // arrange
            var wizardA = new Wizard("Merlin", 10, EnumElementType.Water);
            var orcB = new Orc("Marok Blutklinge", 5, EnumElementType.Fire);

            // act
            var damageA = wizardA.GetDamage(orcB);
            var damageB = orcB.GetDamage(wizardA);

            // assert
            Assert.AreEqual(10,damageA);
            Assert.AreEqual(0,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageOrcVsOrc_BaseDamage()
        {
            // arrange
            var orcA = new Orc("Kuruk Stahlhand", 10, EnumElementType.Normal);
            var orcB = new Orc("Marok Blutklinge", 5, EnumElementType.Fire);

            // act
            var damageA = orcA.GetDamage(orcB);
            var damageB = orcB.GetDamage(orcA);

            // assert
            Assert.AreEqual(10,damageA);
            Assert.AreEqual(5,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageKrakenVsKraken_BaseDamage()
        {
            // arrange
            var krakenA = new Kraken("Cthulu", 50, EnumElementType.Water);
            var krakenB = new Kraken("Giant Squid", 5, EnumElementType.Water);

            // act
            var damageA = krakenA.GetDamage(krakenB);
            var damageB = krakenB.GetDamage(krakenA);

            // assert
            Assert.AreEqual(50,damageA);
            Assert.AreEqual(5,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageBugVsOrc_BugElementDamageOrcBaseDamage()
        {
            // arrange
            var bugA = new Bug("Herbey", 50, EnumElementType.Normal);
            var orcB = new Orc("Marok Blutklinge", 5, EnumElementType.Fire);

            // act
            var damageA = bugA.GetDamage(orcB);
            var damageB = orcB.GetDamage(bugA);

            // assert
            Assert.AreEqual(25,damageA);
            Assert.AreEqual(5,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageBugVsWizard_BugNoDamageWizardBaseDamage()
        {
            // arrange
            var bugA = new Bug("Herbey", 50, EnumElementType.Normal);
            var wizardB = new Wizard("Wolfgang the programmer", 1, EnumElementType.Water);

            // act
            var damageA = bugA.GetDamage(wizardB);
            var damageB = wizardB.GetDamage(bugA);

            // assert
            Assert.AreEqual(0,damageA);
            Assert.AreEqual(1,damageB);
        }

        [Test]
        public void TestMonster_CheckDamageBugVsSpell_BugBaseDamageSpellElementDamage()
        {
            // arrange
            var bugA = new Bug("Herbey", 50, EnumElementType.Normal);
            var spellB = new SpellCard("Tsunami", 40, EnumElementType.Water);

            // act
            var damageA = bugA.GetDamage(spellB);
            var damageB = spellB.GetDamage(bugA);

            // assert
            Assert.AreEqual(50,damageA);
            Assert.AreEqual(20,damageB);
        }

        [Test]
        public void TestSpell_CheckDamageSpellVsSpell_ElementCheck()
        {
            // arrange
            var spellA = new SpellCard("Tsunami", 40, EnumElementType.Water);
            var spellB = new SpellCard("Greater thunder", 30, EnumElementType.Normal);
            var spellC = new SpellCard("Meteor", 50, EnumElementType.Fire);

            // act
            var damageAb = spellA.GetDamage(spellB);
            var damageAc = spellA.GetDamage(spellC);
            var damageAa = spellA.GetDamage(spellA);
            var damageBa = spellB.GetDamage(spellA);
            var damageBc = spellB.GetDamage(spellC);
            var damageBb = spellB.GetDamage(spellB);
            var damageCa = spellC.GetDamage(spellA);
            var damageCb = spellC.GetDamage(spellB);
            var damageCc = spellC.GetDamage(spellC);


            // assert
            Assert.AreEqual(40,damageAa);//Water Same Element - Base Damage
            Assert.AreEqual(30,damageBb);//Normal Same Element - Base Damage
            Assert.AreEqual(50,damageCc);//Fire Same Element - Base Damage

            Assert.AreEqual(20,damageAb);//Water Weak Element - Half Damage
            Assert.AreEqual(15,damageBc);//Normal Weak Element - Half Damage
            Assert.AreEqual(25,damageCa);//Fire Weak Element - Half Damage

            Assert.AreEqual(80,damageAc);//Water Strong Element - Double Damage
            Assert.AreEqual(60,damageBa);//Normal Strong Element - Double Damage
            Assert.AreEqual(100,damageCb);//Fire Strong Element - Double Damage
        }

        [Test]
        public void TestSpell_CheckDamageWaterSpellVsWaterSpell_BaseDamage()
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
        [Test]
        public void TestSpell_CheckDamageFireSpellVsFireSpell_BaseDamage()
        {
            // arrange
            var spellA = new SpellCard("Cinder fog", 3, EnumElementType.Fire);
            var spellB = new SpellCard("Flame spear", 6, EnumElementType.Fire);

            // act
            var damageA = spellA.GetDamage(spellB);
            var damageB = spellB.GetDamage(spellA);


            // assert
            Assert.AreEqual(3,damageA);
            Assert.AreEqual(6,damageB);
        }
        [Test]
        public void TestSpell_CheckDamageNormalSpellVsNormalSpell_BaseDamage()
        {
            // arrange
            var spellA = new SpellCard("Control person", 3, EnumElementType.Normal);
            var spellB = new SpellCard("Thunder shock", 6, EnumElementType.Normal);

            // act
            var damageA = spellA.GetDamage(spellB);
            var damageB = spellB.GetDamage(spellA);


            // assert
            Assert.AreEqual(3,damageA);
            Assert.AreEqual(6,damageB);
        }

        [Test]
        public void TestSpell_CheckDamageKrakenVsSpell_SpellNoDamageKrakenElementDamage()
        {
            // arrange
            var krakenA = new Kraken("Cthulu", 50, EnumElementType.Water);
            var spellB = new SpellCard("Greater thunder", 30, EnumElementType.Normal);

            // act
            var damageA = krakenA.GetDamage(spellB);
            var damageB = spellB.GetDamage(krakenA);

            // assert
            Assert.AreEqual(25,damageA);
            Assert.AreEqual(0,damageB);
        }

        [Test]
        public void TestSpell_CheckDamageKnightVsWaterSpell_SpellDamage999Knight0Damage()
        {
            // arrange
            var knightA = new Knight("Lancelot", 50, EnumElementType.Normal);
            var spellB = new SpellCard("Bubble prison", 2, EnumElementType.Water);

            // act
            var damageA = knightA.GetDamage(spellB);
            var damageB = spellB.GetDamage(knightA);

            // assert
            Assert.AreEqual(0,damageA);
            Assert.AreEqual(999,damageB);
        }

        [Test]
        public void TestSpell_CheckDamageKnightVsNonWaterSpell_ElementDamage()
        {
            // arrange
            var knightA = new Knight("Lancelot", 50, EnumElementType.Normal);
            var spellB = new SpellCard("Cinder fog", 5, EnumElementType.Fire);

            // act
            var damageA = knightA.GetDamage(spellB);
            var damageB = spellB.GetDamage(knightA);

            // assert
            Assert.AreEqual(25,damageA);
            Assert.AreEqual(10,damageB);
        }
    }
}
