﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Store
    {
        public List<TradeCards> MarketCards
        {
            get => default;
            set
            {
            }
        }

        public List<ICardPackage> getCardPackages(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}