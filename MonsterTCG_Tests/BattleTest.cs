using System;
using System.Collections.Generic;
using MonsterTCG;
using NUnit.Framework;
using Moq;


namespace MonsterTCG_Tests
{
    class BattleTest
    {
        [Test]
        public void TestBattle_CreateGameWith2User()
        {
            // arrange
            var mockedUserA = new Mock<User>();
            var mockedUserB = new Mock<User>();

            var users = new List<User>() {mockedUserA.Object, mockedUserB.Object};

            // act
            var battle = new Battle(users);


            // assert
            Assert.AreEqual(users, battle.Player);
            Assert.AreEqual(0, battle.CurrentRound);

        }

        [Test]
        public void TestBattle_CheckWinnerWithEmptyDeck()
        {
            // arrange
            var mockedUserA = new Mock<User>();
            var mockedUserB = new Mock<User>();
            var mockedDeckA = new Mock<CardDeck>();
            var mockedDeckB = new Mock<CardDeck>();
            var mockedCard = new Mock<ICard>();

            mockedUserA.Setup(userA => userA.SelectedDeck).Returns(mockedDeckA.Object);
            mockedDeckA.Setup(deckA => deckA.Cards).Returns(new List<ICard>() { });
            mockedUserB.Setup(userB => userB.SelectedDeck).Returns(mockedDeckB.Object);
            mockedDeckB.Setup(deckB => deckB.Cards).Returns(new List<ICard>() {mockedCard.Object});

            var users = new List<User>() {mockedUserA.Object, mockedUserB.Object};
            var battle = new Battle(users);

            // act
            


            // assert
            Assert.AreEqual(mockedUserB.Object,battle.GetWinner());

        }

        [Test]
        public void TestBattle_CheckWinnerWithFullDecks()
        {
            // arrange
            var mockedUserA = new Mock<User>();
            var mockedUserB = new Mock<User>();
            var mockedDeckA = new Mock<CardDeck>();
            var mockedDeckB = new Mock<CardDeck>();
            var mockedCard = new Mock<ICard>();

            mockedUserA.Setup(userA => userA.SelectedDeck).Returns(mockedDeckA.Object);
            mockedDeckA.Setup(deckA => deckA.Cards).Returns(new List<ICard>() {mockedCard.Object, mockedCard.Object, mockedCard.Object, mockedCard.Object});
            mockedUserB.Setup(userB => userB.SelectedDeck).Returns(mockedDeckB.Object);
            mockedDeckB.Setup(deckB => deckB.Cards).Returns(new List<ICard>() {mockedCard.Object, mockedCard.Object, mockedCard.Object, mockedCard.Object});

            var users = new List<User>() {mockedUserA.Object, mockedUserB.Object};
            var battle = new Battle(users);

            // act

            // assert
            Assert.AreEqual(null,battle.GetWinner());

        }

    }
}
