using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Wizard:MonsterCard
    {
        public Wizard(string name, int damage, EnumElementType element) : base(name, damage, element)
        {
        }
    }
}