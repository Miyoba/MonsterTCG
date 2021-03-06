﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace MonsterTCG
{
    public class User
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Coins")]
        public int Coins { get; set; }

        [JsonProperty("Bio")]
        public string Bio { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("Elo")]
        public int Elo { get; set; }
        public CardDeck SelectedDeck { get; set; }
        public List<BattleLog> Logs { get; set; }
    }
}