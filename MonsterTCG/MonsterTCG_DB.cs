using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class MonsterTcgdb
    {
        public MonsterTcgdb(string hostname, string pass, string username, string password)
        {
            _hostname = hostname;
            _username = username;
            _password = password;
        }

        private string _hostname;

        private string _password;

        private string _username;

        public void connect()
        {
            throw new System.NotImplementedException();
        }
    }
}