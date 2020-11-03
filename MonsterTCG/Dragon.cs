using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Dragon:MonsterCard
    {
        public Dragon(string name, int damage, EnumElementType element) : base(name, damage, element)
        {
        }

        public override int GetDamage(ICard enemy)
        {
            if (enemy is Fireelf)
                return 0;
            return base.GetDamage(enemy);
        }
    }
}