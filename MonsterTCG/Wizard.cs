namespace MonsterTCG
{
    public class Wizard:MonsterCard
    {
        public Wizard(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public Wizard(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
    }
}