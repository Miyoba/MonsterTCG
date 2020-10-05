using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Server
    {

        public System.Collections.Generic.Dictionary<string, int> Credentials
        {
            get => default;
            set
            {
            }
        }

        public MonsterTCGDB Db
        {
            get => default;
            set
            {
            }
        }

        public Store Store
        {
            get => default;
            set
            {
            }
        }

        public Game Game
        {
            get => default;
            set
            {
            }
        }

        /// <param name="username">The Username of the User</param>
        public Boolean createUser(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Boolean connectDB()
        {
            throw new System.NotImplementedException();
        }

        public void createGame(List<User> users, List<CardDeck> decks)
        {
            throw new System.NotImplementedException();
        }
    }
}