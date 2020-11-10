﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Kraken:MonsterCard
    {
        public Kraken(string name, int damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public override int GetDamage(ICard enemy)
        {
            if (enemy is SpellCard)
                return Element.GetDamage(enemy, Damage);
            return Damage;
        }
    }
}