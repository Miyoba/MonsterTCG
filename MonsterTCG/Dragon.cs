using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Dragon:MonsterCard
    {
        public Dragon(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }

        public Dragon(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
        public override double GetDamage(ICard enemy)
        {
            if (enemy is Fireelf)
                return 0;
            return base.GetDamage(enemy);
        }
    }
}