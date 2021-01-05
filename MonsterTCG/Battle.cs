using System;
using System.Collections.Generic;

namespace MonsterTCG
{
    public class Battle
    {
        public Battle(List<User> users, int maxRounds)
        {
            if (maxRounds <= 0)
                throw new ArgumentException("Only rounds above 0 are allowed!");

            if(users.Count <= 1)
                throw new ArgumentException("A battle needs a minimum of 2 user!");


            MaxRounds = maxRounds;
            CurrentRound = 0;
            BattleDecks = new List<BattleDeck>();
            foreach (var user in users)
            {
                BattleDecks.Add(new BattleDeck(user, user.SelectedDeck));
            }

            Log = new BattleLog();
        }

        public Battle(List<User> users)
        {
            MaxRounds = 100;
            CurrentRound = 0;
            BattleDecks = new List<BattleDeck>();
            foreach (var user in users)
            {
                BattleDecks.Add(new BattleDeck(user, user.SelectedDeck));
            }

            Log = new BattleLog();
        }

        public int MaxRounds { get; set; }

        public int CurrentRound { get; set; }

        public List<BattleDeck> BattleDecks { get; set; }

        public BattleLog Log { get; set; }

        public BattleLog Fight()
        {
            while (CurrentRound != 100 && Log.Winner == null)
            {
                Log.AddRoundText(NextRound());
                Log.Winner = GetWinner();
            }

            if (Log.Winner != null)
                Log.AddRoundText("#######################################\nThe winner of this battle is " +
                                 Log.Winner.Username.ToUpper() + "\n");
            else
                Log.AddRoundText("#######################################\nNobody was able to beat the opponents deck!\n");
            return Log;
        }

        public string NextRound()
        {
            CurrentRound += 1;
            string roundText = "Round "+CurrentRound+": ";

            double damage0 = BattleDecks[0].Cards[0].GetDamage(BattleDecks[1].Cards[0]);
            double damage1 = BattleDecks[1].Cards[0].GetDamage(BattleDecks[0].Cards[0]);

            if (damage0 > damage1)
            {
                roundText += BattleDecks[0].User.Username + "'s " + BattleDecks[0].Cards[0].Name + " with " + damage0 +
                       " damage overwhelms " + BattleDecks[1].User.Username + "'s " + BattleDecks[1].Cards[0].Name +
                       " with only " + damage1 + " damage.";
                BattleDecks[0].AddCard(BattleDecks[1].Cards[0]);
                BattleDecks[1].RemoveCard(BattleDecks[1].Cards[0]);
                BattleDecks[0].MoveFirstCardToLastPos();
            }

            else if (damage0 < damage1)
            {
                roundText += BattleDecks[1].User.Username + "'s " + BattleDecks[1].Cards[0].Name + " with " + damage1 +
                       " damage overwhelms " + BattleDecks[0].User.Username + "'s " + BattleDecks[0].Cards[0].Name +
                       " with only " + damage0 + " damage.";
                BattleDecks[1].AddCard(BattleDecks[0].Cards[0]);
                BattleDecks[0].RemoveCard(BattleDecks[0].Cards[0]);
                BattleDecks[1].MoveFirstCardToLastPos();
            }

            else if (Math.Abs(damage0 - damage1) < 0.01)
            {
                roundText += BattleDecks[1].User.Username + "'s " + BattleDecks[1].Cards[0].Name + " and " +
                       BattleDecks[0].User.Username + "'s " + BattleDecks[0].Cards[0].Name +
                       " clash together with an equal strength of " + damage0 + ".";
                BattleDecks[1].MoveFirstCardToLastPos();
                BattleDecks[0].MoveFirstCardToLastPos();
            }

            roundText += "\n" + BattleDecks[0].User.Username + "'s Deck counter: " + BattleDecks[0].Cards.Count +
                         "\t|\t" + BattleDecks[1].User.Username + "'s Deck counter: " + BattleDecks[1].Cards.Count+"\n";

            return roundText;
        }

        public User GetWinner()
        {
            User winner = null;
            foreach (var deck in BattleDecks)
            {
                if (deck.Cards.Count > 0)
                {
                    if (winner != null)
                        return null;
                    winner = deck.User;
                }
            }
            return winner;
        }
    }
}