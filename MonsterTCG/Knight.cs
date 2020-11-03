using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Knight:MonsterCard
    {
        public Knight(string name, int damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public override int GetDamage(ICard enemy)
        {
            if (enemy is SpellCard && enemy.Element is Water)
                return -1;
            return base.GetDamage(enemy);
        }
    }
}