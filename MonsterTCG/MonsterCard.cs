namespace MonsterTCG
{
    public class MonsterCard:ICard
    {
        public MonsterCard(string name, double damage, EnumElementType element)
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

        public MonsterCard(string id, string name, double damage, EnumElementType element)
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

        public virtual double GetDamage(ICard enemy)
        {
            if (!(enemy is SpellCard))
                return Damage;
            return Element.GetDamage(enemy, Damage);
        }
    }
}