using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public interface IElement
    {
        double GetDamage(ICard enemy, double damage);
    }
}