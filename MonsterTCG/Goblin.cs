using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace MonsterTCG
{
    public class Goblin:MonsterCard
    {
        public Goblin(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public Goblin(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
        public override double GetDamage(ICard enemy)
        {
            if (enemy is Dragon)
                return 0;
            return base.GetDamage(enemy);
        }
    }
}