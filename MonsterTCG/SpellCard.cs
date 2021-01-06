namespace MonsterTCG
{
    public class SpellCard:ICard
    {
        public SpellCard(string name, double damage, EnumElementType element)
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
        public SpellCard(string id, string name, double damage, EnumElementType element)
        {
            Id = id;
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
        public string Id { get; set; }

        public double Damage { get; set; }

        public IElement Element { get; set; }

        public string Name { get; set; }
        public double GetDamage(ICard enemy)
        {
            if (enemy is Kraken)
                return 0;
            if(enemy is Knight && Element is Water)
                return 999;
            return Element.GetDamage(enemy, Damage);
        }
    }
}