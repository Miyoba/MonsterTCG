using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace MonsterTCG
{
    public class MonsterCard:ICard
    {
        public EnumMonsterType Type { get; set; }

        public int Damage { get; set; }

        public EnumElementType Element { get; set; }

        public string Name { get; set; }
        
    }
}