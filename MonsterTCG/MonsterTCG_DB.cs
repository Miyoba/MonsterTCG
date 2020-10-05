using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class MonsterTCGDB
    {
        public MonsterTCGDB(string hostname, string pass, string username)
        {
            throw new System.NotImplementedException();
        }

        public string Hostname
        {
            get => default;
            set
            {
            }
        }

        public string Password
        {
            get => default;
            set
            {
            }
        }

        public string Username
        {
            get => default;
            set
            {
            }
        }

        public void connect()
        {
            throw new System.NotImplementedException();
        }

        public Directory<string, int> getUserCredentials(string username)
        {
            throw new System.NotImplementedException();
        }

        public User getUserInformation(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}