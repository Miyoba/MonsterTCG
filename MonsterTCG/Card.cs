using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MonsterTCG
{

    public interface ICard
    {
        string Id { get; set; }
        double Damage { get; set; }
        IElement Element { get; set; }
        string Name { get; set; }
        double GetDamage(ICard enemy);
    }
}