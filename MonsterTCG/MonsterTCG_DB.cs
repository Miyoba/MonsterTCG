using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace MonsterTCG
{
    public class MonsterTcgdb
    {
        static readonly string ConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        public bool RegisterUser(string username, string password)
        {
            string token = "Basic "+username+"-mtcgToken";

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("INSERT INTO credentials (username, password, token) VALUES (@username, @password, @token)", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("token", token);
            cmd.Prepare();

            var cmd2 = new NpgsqlCommand("INSERT INTO player (username, coins, elo) VALUES (@username, @coins, @elo)", conn);
            cmd2.Parameters.AddWithValue("username", username);
            cmd2.Parameters.AddWithValue("coins", 20);
            cmd2.Parameters.AddWithValue("elo", 100);
            cmd2.Prepare();

            try
            {
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
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
            catch (Npgsql.PostgresException)
            {
                return null;
            }
        }

        public bool CreatePackage(string package)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("INSERT INTO packages (cards_json) VALUES (@json)", conn);
            cmd.Parameters.AddWithValue("json", package);
            cmd.Prepare();
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Npgsql.PostgresException)
            {
                return false;
            }
        }

        public string BuyPackage(string token)
        {

            string erg = null;
            int coins = 0;
            string username = GetUsernameFromToken(token);
            List<ICard> realCards = new List<ICard>();

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();


            var cmd = new NpgsqlCommand("SELECT coins FROM player where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            var cmd0 = new NpgsqlCommand("SELECT cards_json FROM packages where id IN (SELECT id from packages ORDER BY id asc LIMIT 1)", conn);
            cmd0.Prepare();

            try
            {
                // ReSharper disable once PossibleNullReferenceException
                coins = (int)cmd.ExecuteScalar();

                if(coins < 5)
                    return null;

                coins -= 5;

                erg = (string)cmd0.ExecuteScalar();

                if(erg == null)
                    return null;

                List<JsonCard> cards = JsonConvert.DeserializeObject<List<JsonCard>>(erg);

                foreach (var card in cards)
                {
                    realCards.Add(card.ConvertToCard());
                }

            }
            catch (Npgsql.PostgresException)
            {
                return null;
            }

            cmd0 = new NpgsqlCommand("UPDATE player SET coins = @coins where username = @username", conn);
            cmd0.Parameters.AddWithValue("username", username);
            cmd0.Parameters.AddWithValue("coins", coins);
            cmd0.Prepare();

            var cmd1 = new NpgsqlCommand("DELETE FROM packages where id IN (SELECT id from packages ORDER BY id asc LIMIT 1)", conn);
            cmd1.Prepare();

            try
            {
                int check = cmd0.ExecuteNonQuery();
                if (check == -1)
                    return null;

                cmd1.ExecuteNonQuery();
            }

            catch (Npgsql.PostgresException)
            {
                return null;
            }

            foreach (var card in realCards)
            {
                var cmd2 = new NpgsqlCommand("INSERT INTO cards (id, name, typ, element, damage) VALUES (@id, @name, @typ, @element, @damage)", conn);
                cmd2.Parameters.AddWithValue("id", card.Id);
                cmd2.Parameters.AddWithValue("name", card.Name);
                cmd2.Parameters.AddWithValue("typ", card.GetType().FullName);
                cmd2.Parameters.AddWithValue("element", card.Element.ToString());
                cmd2.Parameters.AddWithValue("damage", card.Damage);
                cmd2.Prepare();

                var cmd3 = new NpgsqlCommand("INSERT INTO player_cards (username, card_id) VALUES (@username, @id)", conn);
                cmd3.Parameters.AddWithValue("id", card.Id);
                cmd3.Parameters.AddWithValue("username", username);
                cmd3.Prepare();

                try
                {
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                }
                catch (Npgsql.PostgresException)
                {
                    return null;
                }
            }
            return erg;
        }

        public List<JsonCard> ShowCards(string token)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string username = GetUsernameFromToken(token);
            List<JsonCard> cards = new List<JsonCard>();

            var cmd = new NpgsqlCommand("Select cards.id, name, damage from cards inner join player_cards on cards.id = player_cards.card_id where player_cards.username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var temp = new JsonCard();
                        temp.Id = (string) reader[0];
                        temp.Name = (string) reader[1];
                        temp.Damage = (double)((decimal)reader[2]);
                        cards.Add(temp);
                    }
                    return cards;
                }
                return null;
            }
            catch (Npgsql.PostgresException)
            {
                return null;
            }
        }

        public List<string> ShowDeck(string username)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("Select card_id from player_deck where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<string> cards = new List<string>();
                    while (reader.Read())
                    {
                        var temp = (string) reader[0];
                        cards.Add(temp);
                    }
                    return cards;
                }
                return null;
            }
            catch (Npgsql.PostgresException)
            {
                return null;
            }
        }

        public string ConfigureDeck(string username, List<string> cards)
        {
            
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            List<string> checkList = new List<string>();
            try
            {
                foreach (var card in cards)
                {
                    //Check if cards are owned
                    var cmd = new NpgsqlCommand("Select * from player_cards where username = @username and card_id = @card_id", conn);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("card_id", card);
                    cmd.Prepare();

                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                        return "Cards are not found in the users stack!";

                    reader.Close();

                    //Check if cards are in a trade request
                    cmd = new NpgsqlCommand("Select * from trade where card_id = @card_id", conn);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("card_id", card);
                    cmd.Prepare();

                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                        return "Deck could not be configured since some cards are currently in a trade request!";

                    reader.Close();
                }

                //Delete current deck
                var cmd1 = new NpgsqlCommand("DELETE FROM player_deck where username = @username", conn);
                cmd1.Parameters.AddWithValue("username", username);
                cmd1.Prepare();

                
                cmd1.ExecuteNonQuery();


                foreach (var card in cards)
                {
                    //Add cards to player_deck
                    var cmd3 = new NpgsqlCommand("INSERT INTO player_deck (username, card_id) VALUES (@username, @card_id)", conn);
                    cmd3.Parameters.AddWithValue("username", username);
                    cmd3.Parameters.AddWithValue("card_id", card);
                    cmd3.Prepare();

                    cmd3.ExecuteNonQuery();
                }
            }
            catch (Npgsql.PostgresException)
            {
                return "Internal database error!";
            }

            return null;
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

        public string GetUsernameFromToken(string token)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT username FROM credentials where token = @token", conn);
            cmd.Parameters.AddWithValue("token", token);
            cmd.Prepare();

            try
            {
                return (string)cmd.ExecuteScalar();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}