using Newtonsoft.Json;

namespace MonsterTCG
{
    public class TradeCards
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("CardToTrade")]
        public string CardId { get; set; }

        [JsonProperty("Type")]
        public string RequirementType { get; set; }
        [JsonProperty("Element")]
        public string RequirementElement { get; set; }

        [JsonProperty("MinimumDamage")]
        public double RequirementDamage { get; set; }

        
    }
}