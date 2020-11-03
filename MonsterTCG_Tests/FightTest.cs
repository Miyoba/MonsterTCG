using System;
using System.Collections.Generic;
using MonsterTCG;
using NUnit.Framework;
using Moq;


namespace MonsterTCG_Tests
{
    class FightTest
    {
        [Test]
        public void TestFight_FightWith2NormalDragons()
        {
            // arrange
            var mockedUserA = new Mock<User>();
            var mockedUserB = new Mock<User>();

            var users = new List<User>() {mockedUserA.Object, mockedUserB.Object};

            // act
            //var battle = new Battle(users);
            //battle.NextRound();


            // assert
            Assert.Pass();

        }
    }
}
