using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{

    public interface ICard
    {
        int Damage { get; set; }
        IElement Element { get; set; }
        string Name { get; set; }
        int GetDamage(ICard enemy);
    }
}