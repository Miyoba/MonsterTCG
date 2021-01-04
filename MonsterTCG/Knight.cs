namespace MonsterTCG
{
    public class Knight:MonsterCard
    {
        public Knight(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public Knight(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
        public override double GetDamage(ICard enemy)
        {
            if (enemy is SpellCard && enemy.Element is Water)
                return -1;
            return base.GetDamage(enemy);
        }
    }
}