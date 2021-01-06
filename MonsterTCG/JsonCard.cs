using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MonsterTCG
{
    public class JsonCard
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Damage")]
        public double Damage { get; set; }

        public ICard ConvertToCard()
        {
            EnumElementType elementType = EnumElementType.Normal;
            if (Regex.IsMatch(Name, "Fire.*"))
                elementType = EnumElementType.Fire;
            if (Regex.IsMatch(Name, "Water.*"))
                elementType = EnumElementType.Water;
            
            if (Regex.IsMatch(Name, ".*Goblin")) 
                return new Goblin(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Ork"))
                return new Orc(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Dragon"))
                return new Dragon(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Knight"))
                return new Knight(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Wizard"))
                return new Wizard(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Elf"))
                return new Fireelf(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Kraken"))
                return new Kraken(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Bug"))
                return new Bug(Id, Name, Damage, elementType);
            if (Regex.IsMatch(Name, ".*Spell"))
                return new SpellCard(Id, Name, Damage, elementType);
            return null;
        }
    }
}