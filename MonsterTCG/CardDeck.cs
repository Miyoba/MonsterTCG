﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class CardDeck
    {
        public List<ICard> Cards { get; set; }

        public int Size { get; set; }
    }
}