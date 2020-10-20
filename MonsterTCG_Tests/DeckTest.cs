using System;
using System.Collections.Generic;
using MonsterTCG;
using NUnit.Framework;
using Moq;


namespace MonsterTCG_Tests
{
    public class DeckTest
    {
        
        [Test]
        public void TestDeck_Create4CardsDeck()
        {
            // arrange
            var mockedA = new Mock<ICard>();
            var mockedB = new Mock<ICard>();
            var mockedC = new Mock<ICard>();
            var mockedD = new Mock<ICard>();

            var cards = new List<ICard> {mockedA.Object, mockedB.Object, mockedC.Object, mockedD.Object};

            // act
            var deck = new CardDeck(cards, 4);

            // assert
            Assert.AreEqual(cards, deck.Cards);

        }

        [Test]
        public void TestDeck_Create5CardsDeckAndThrowException()
        {
            // arrange
            var mockedA = new Mock<ICard>();
            var mockedB = new Mock<ICard>();
            var mockedC = new Mock<ICard>();
            var mockedD = new Mock<ICard>();
            var mockedE = new Mock<ICard>();

            var cards = new List<ICard> {mockedA.Object, mockedB.Object, mockedC.Object, mockedD.Object, mockedE.Object};

            // assert
            Assert.Throws<ArgumentException>(()=>new CardDeck(cards,4));

        }

        [Test]
        public void TestDeck_CreateEmptyDeck()
        {
            // arrange
            // act
            var deck = new CardDeck();

            // assert
            Assert.AreEqual(new List<ICard>(), deck.Cards);
            Assert.IsFalse(deck.Legal);

        }

        [Test]
        public void TestDeck_AddCardInEmptyDeck()
        {
            // arrange
            var mockedA = new Mock<ICard>();
            var deck = new CardDeck();
            var cards = new List<ICard> {mockedA.Object};

            // act
            var erg = deck.AddCard(mockedA.Object);

            // assert
            Assert.IsTrue(erg);
            Assert.AreEqual(cards, deck.Cards);
        }

        [Test]
        public void TestDeck_AddCardInFullDeckAndThrowException()
        {
            // arrange
            var mockedA = new Mock<ICard>();
            var mockedB = new Mock<ICard>();
            var mockedC = new Mock<ICard>();
            var mockedD = new Mock<ICard>();
            var mockedE = new Mock<ICard>();

            var cards = new List<ICard> {mockedA.Object, mockedB.Object, mockedC.Object, mockedD.Object};
            var deck = new CardDeck(cards, 4);

            // act and assert
            Assert.Throws<ArgumentException>(()=>deck.AddCard(mockedE.Object));
        }

        [Test]
        public void TestDeck_RemoveCardInFullDeck()
        {
            // arrange
            var mockedA = new Mock<ICard>();
            var mockedB = new Mock<ICard>();
            var mockedC = new Mock<ICard>();
            var mockedD = new Mock<ICard>();

            var cards = new List<ICard> {mockedA.Object, mockedB.Object, mockedC.Object, mockedD.Object};
            var deck = new CardDeck(cards, 4);

            // act
            deck.RemoveCard(mockedA.Object);
            cards.Remove(mockedA.Object);

            // assert
            Assert.AreEqual(cards, deck.Cards);
            Assert.IsFalse(deck.Legal);
        }

        [Test]
        public void TestDeck_RemoveCardInEmptyDeck()
        {
            // arrange
            var mockedA = new Mock<ICard>();
            var deck = new CardDeck(4);

            // act and assert
            Assert.Throws<ArgumentException>(()=>deck.RemoveCard(mockedA.Object));
        }
    }
}