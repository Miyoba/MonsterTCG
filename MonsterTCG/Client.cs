using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class Client
    {

        public Server Server
        {
            get => default;
            set
            {
            }
        }

        public String Username
        {
            get => default;
            set
            {
            }
        }

        public int PasswordHash
        {
            get => default;
            set
            {
            }
        }

        public String AuthToken
        {
            get => default;
            set
            {
            }
        }

        public User User
        {
            get => default;
            set
            {
            }
        }

        public Server connect()
        {
            throw new System.NotImplementedException();
        }

        public void buyCardPackage()
        {
            throw new System.NotImplementedException();
        }
    }
}