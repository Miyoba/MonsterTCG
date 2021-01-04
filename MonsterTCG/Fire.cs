using System;

namespace MonsterTCG
{
    public class Fire:IElement
    {
        public double GetDamage(ICard enemy, double damage)
        {
            if (enemy.Element is Normal) 
                return damage * 2;

            if(enemy.Element is Fire) 
                return damage;

            if (enemy.Element is Water)
                return damage/2;

            throw new ArgumentException("Unknown element encountered while in damage calculation");
        }
    }
}