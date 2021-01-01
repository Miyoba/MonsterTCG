using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (Regex.IsMatch(Name, ".*Goblin"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Goblin(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Goblin(Id, Name, Damage, EnumElementType.Water);
                return new Goblin(Id, Name, Damage, EnumElementType.Normal);
            }
            if (Regex.IsMatch(Name, ".*Ork"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Orc(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Orc(Id, Name, Damage, EnumElementType.Water);
                return new Orc(Id, Name, Damage, EnumElementType.Normal);
            }
            if (Regex.IsMatch(Name, ".*Dragon"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Dragon(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Dragon(Id, Name, Damage, EnumElementType.Water);
                return new Dragon(Id, Name, Damage, EnumElementType.Normal);
            }
            if (Regex.IsMatch(Name, ".*Knight"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Knight(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Knight(Id, Name, Damage, EnumElementType.Water);
                return new Knight(Id, Name, Damage, EnumElementType.Normal);
            }
            if (Regex.IsMatch(Name, ".*Wizard"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Wizard(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Wizard(Id, Name, Damage, EnumElementType.Water);
                return new Wizard(Id, Name, Damage, EnumElementType.Normal);
            }
            if (Regex.IsMatch(Name, ".*Elf"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Fireelf(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Fireelf(Id, Name, Damage, EnumElementType.Water);
                return new Fireelf(Id, Name, Damage, EnumElementType.Normal);
            }
            if (Regex.IsMatch(Name, ".*Kraken"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Kraken(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Kraken(Id, Name, Damage, EnumElementType.Water);
                return new Kraken(Id, Name, Damage, EnumElementType.Normal);
            }
            if (Regex.IsMatch(Name, ".*Spell"))
            {
                if (Regex.IsMatch(Name, "Fire.*"))
                    return new Knight(Id, Name, Damage, EnumElementType.Fire);
                if (Regex.IsMatch(Name, "Water.*"))
                    return new Knight(Id, Name, Damage, EnumElementType.Water);
                return new Knight(Id, Name, Damage, EnumElementType.Normal);
            }

            return null;
        }
    }
}