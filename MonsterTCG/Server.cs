using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Server
    {
        public Server(string hostname, string username, string password, string dbName, int port)
        {
            throw new System.NotImplementedException();
        }

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

        public Battle Game
        {
            get => default;
            set
            {
            }
        }

        /// <param name="username">The Username of the User</param>
        public Boolean createUser(string username, int passwordHash)
        {
            throw new System.NotImplementedException();
        }

        public Boolean connectDB(string hostname, string username, string password, string dbName, int port)
        {
            throw new System.NotImplementedException();
        }

        public void createGame(List<User> users, List<CardDeck> decks)
        {
            throw new System.NotImplementedException();
        }

        public User loginUser(string username, int passwordHash)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, int> getAllUserCredentials()
        {
            throw new System.NotImplementedException();
        }

        public Boolean updateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}