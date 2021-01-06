using System.Collections.Generic;

namespace MonsterTCG
{
    public class BattleDeck
    {
        public BattleDeck(User user, CardDeck deck)
        {
            Cards = new List<ICard>();
            User = user;
            foreach (var card in deck.Cards)
            {
                Cards.Add(card);
            }
        }

        public User User { get; set; }

        public List<ICard> Cards { get; set; }

        public void AddCard(ICard card)
        {
            Cards.Add(card);
        }

        public bool RemoveCard(ICard card)
        {
            return Cards.Remove(card);
        }

        public void MoveFirstCardToLastPos()
        {
            ICard card = Cards[0];
            Cards.Remove(card);
            Cards.Add(card);
        }
    }
}