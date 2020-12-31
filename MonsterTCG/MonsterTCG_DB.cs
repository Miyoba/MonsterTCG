using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace MonsterTCG
{
    public class MonsterTcgdb
    {
        static readonly string ConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        public bool RegisterUser(string username, string password)
        {
            string token = username+"-mtcgToken";

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("INSERT INTO credentials (username, password, token) VALUES (@username, @password, @token)", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("token", token);
            cmd.Prepare();
            try
            {
                var count = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Npgsql.PostgresException)
            {
                return false;
            }
        }

        public string LoginUser(string username, string password)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT token FROM credentials where username = @username and password = @password", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Prepare();

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return (string) reader[0];
                }

                return null;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool CreatePackage()
        {
            throw new NotImplementedException();
        }

        public bool BuyPackage()
        {
            throw new NotImplementedException();
        }

        public List<ICard> ShowCards()
        {
            throw new NotImplementedException();
        }

        public List<ICard> ShowDeck()
        {
            throw new NotImplementedException();
        }

        public bool ConfigureDeck()
        {
            throw new NotImplementedException();
        }

        public User ShowPlayerData()
        {
            throw new NotImplementedException();
        }

        public bool EditPlayer()
        {
            throw new NotImplementedException();
        }

        public string ShowStats()
        {
            throw new NotImplementedException();
        }
        public string ShowScoreboard()
        {
            throw new NotImplementedException();
        }
        public bool CreateTrade()
        {
            throw new NotImplementedException();
        }

        public bool ExecuteTrade()
        {
            throw new NotImplementedException();
        }

        public string DeleteTrade()
        {
            throw new NotImplementedException();
        }

        public string ShowAvailableTrades()
        {
            throw new NotImplementedException();
        }
    }
}