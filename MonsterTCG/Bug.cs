namespace MonsterTCG
{
    public class Bug:MonsterCard
    {
        public Bug(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public Bug(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
        public override double GetDamage(ICard enemy)
        {
            if (enemy is Wizard)
                return 0;
            if (enemy is MonsterCard)
                return Element.GetDamage(enemy, Damage);
            return Damage;
        }
    }
}