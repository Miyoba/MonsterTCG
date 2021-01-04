namespace MonsterTCG
{
    public class Fireelf : MonsterCard
    {
        public Fireelf(string name, double damage, EnumElementType element) : base(name, damage, element)
        {
        }
        public Fireelf(string id, string name, double damage, EnumElementType element) : base(id, name, damage, element)
        {
        }
    }
}