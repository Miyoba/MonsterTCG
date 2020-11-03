using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Orc:MonsterCard
    {
        public Orc(string name, int damage, EnumElementType element) : base(name, damage, element)
        {
        }

        public override int GetDamage(ICard enemy)
        {
            if (enemy is Wizard)
                return 0;
            return base.GetDamage(enemy);
        }
    }
}