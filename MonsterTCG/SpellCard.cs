using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class SpellCard:ICard
    {
        public int Damage { get; set; }
        public EnumElementType Element { get; set; }
        public string Name { get; set; }
    }
}