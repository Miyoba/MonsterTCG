using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Stack
    {
        public List<CardDeck> Decks
        {
            get => default;
            set
            {
            }
        }

        public List<ICard> CardCollection
        {
            get => default;
            set
            {
            }
        }

        public List<ICard> AddCards()
        {
            throw new System.NotImplementedException();
        }
    }
}