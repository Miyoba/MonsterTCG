using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Normal:IElement
    {
        public int GetDamage(ICard enemy, int damage)
        {
            if (enemy.Element is Normal) 
                return damage;

            if(enemy.Element is Fire) 
                return (int)damage/2;

            if (enemy.Element is Water)
                return damage*2;

            throw new ArgumentException("Unknown element encountered while in damage calculation");
        }
    }
}