using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class CardDeck
    {
        public CardDeck(List<ICard> cards)
        {
            throw new System.NotImplementedException();
        }

        public CardDeck()
        {
            throw new System.NotImplementedException();
        }

        public List<ICard> Cards { get; set; }

        public int Size { get; set; }

        public bool Legal
        {
            get => default;
            set
            {
            }
        }

        public string DeckName
        {
            get => default;
            set
            {
            }
        }

        public bool addCard(ICard card)
        {
            throw new System.NotImplementedException();
        }

        public bool removeCard(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}