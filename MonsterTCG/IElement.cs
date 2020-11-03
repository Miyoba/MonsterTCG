using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public interface IElement
    {
        int GetDamage(ICard enemy, int damage);
    }
}