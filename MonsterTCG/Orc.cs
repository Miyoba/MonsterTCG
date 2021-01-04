namespace MonsterTCG
{
    public class Orc:MonsterCard
    {
        public Orc(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public Orc(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
        public override double GetDamage(ICard enemy)
        {
            if (enemy is Wizard)
                return 0;
            return base.GetDamage(enemy);
        }
    }
}