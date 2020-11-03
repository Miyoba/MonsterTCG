using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Battle
    {
        public Battle(List<User> user, int maxRounds)
        {
            throw new System.NotImplementedException();
        }

        public Battle(List<User> player)
        {
            throw new System.NotImplementedException();
        }

        public List<User> Player
        {
            get => default;
            set
            {
            }
        }

        public int MaxRounds
        {
            get => default;
            set
            {
            }
        }

        public int CurrentRound
        {
            get => default;
            set
            {
            }
        }

        public Dictionary<User, BattleDeck> battleDecks
        {
            get => default;
            set
            {
            }
        }

        public BattleLog Fight()
        {
            throw new System.NotImplementedException();
        }

        public string NextRound()
        {
            throw new System.NotImplementedException();
        }

        public User GetWinner()
        {
            throw new System.NotImplementedException();
        }
    }
}