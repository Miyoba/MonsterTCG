namespace MonsterTCG
{
    public class Goblin:MonsterCard
    {
        public Goblin(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public Goblin(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
        public override double GetDamage(ICard enemy)
        {
            if (enemy is Dragon)
                return 0;
            return base.GetDamage(enemy);
        }
    }
}