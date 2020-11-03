using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class SpellCard:ICard
    {
        public SpellCard(string name, int damage, EnumElementType element)
        {
            Name = name;
            Damage = damage;
            switch (element)
            {
                case EnumElementType.Normal:
                    Element = new Normal();
                    break;
                case EnumElementType.Fire:
                    Element = new Fire();
                    break;
                case EnumElementType.Water:
                    Element = new Water();
                    break;
                default:
                    Element = new Normal();
                    break;
            }
        }

        public int Damage { get; set; }
        public IElement Element { get; set; }
        public string Name { get; set; }
        public int GetDamage(ICard enemy)
        {
            if (enemy is Kraken)
                return -1;
            if(enemy is Knight && Element is Water)
                return 999;
            return Element.GetDamage(enemy, Damage);
        }
    }
}