using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class CardDeck
    {
        public CardDeck(List<ICard> cards, int size)
        {
            MaxSize = 4;
            if(cards.Count > MaxSize)
                throw new ArgumentException("Too many cards in the Deck.");
            Cards = cards;
            Legal = Cards.Count == MaxSize;
        }

        public CardDeck()
        {
            MaxSize = 4;
            Cards = new List<ICard>();
            Legal = false;
        }

        public CardDeck(int size)
        {
            MaxSize = size;
            Cards = new List<ICard>();
            Legal = false;
        }

        public List<ICard> Cards { get; set; }

        public int MaxSize { get; set; }

        public bool Legal { get; set; }

        public string DeckName { get; set; }

        public bool AddCard(ICard card)
        {
            if (Cards.Count < MaxSize)
            {
                Cards.Add(card);
                if (MaxSize == Cards.Count)
                    Legal = true;
                return true;
            }

            throw new ArgumentException("Can't add a card in a deck that reached its maximum capacity.");
        }

        public bool RemoveCard(ICard card)
        {
            if(!Cards.Contains(card))
                throw new ArgumentException("No card found to remove in deck.");
            
            Cards.Remove(card);
            Legal = false;
            return true;

        }
    }
}