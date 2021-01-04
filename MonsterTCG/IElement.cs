namespace MonsterTCG
{
    public interface IElement
    {
        double GetDamage(ICard enemy, double damage);
    }
}