using System;
using System.Collections.Generic;
using System.IO;
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

        public JsonUser ShowPlayerData(string username)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("Select username, name, coins, image, bio, elo from player where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            JsonUser user = new JsonUser();

            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user.Username = (string)reader[0];
                    if(!(reader[1] is DBNull))
                        user.Name = (string) reader[1];
                    user.Coins = (int) reader[2];
                    if(!(reader[3] is DBNull))
                        user.Image = (string) reader[3];
                    if(!(reader[4] is DBNull))
                        user.Bio = (string) reader[4];
                    user.Elo = (int) reader[5];
                }

                return user;
            }
            catch (Npgsql.PostgresException)
            {
                return null;
            }

            
        }

        public bool EditPlayer(JsonUser user)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("UPDATE player SET name = @name, bio = @bio, image = @image where username = @username", conn);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("name", user.Name);
            cmd.Parameters.AddWithValue("bio", user.Bio);
            cmd.Parameters.AddWithValue("image", user.Image);
            cmd.Prepare();

            try
            {
                if (cmd.ExecuteNonQuery() != -1)
                    return true;
                return false;
            }
            catch (Npgsql.PostgresException)
            {
                return false;
            }
        }

        public string ShowStats(string username)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("Select elo from player where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();


            try
            {
                // ReSharper disable once PossibleNullReferenceException
                var reader = (int) cmd.ExecuteScalar();
                
                return username +"'s Elo: "+ reader;
            }
            catch (Npgsql.PostgresException)
            {
                return null;
            }
        }
        public string ShowScoreboard()
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("Select username, elo from player where username != 'admin' order by elo", conn);
            cmd.Prepare();


            try
            {
                // ReSharper disable once PossibleNullReferenceException
                var reader = cmd.ExecuteReader();
                string scoreboard = "\tusername\t|\tElo\n";
                scoreboard += "############################\n";
                while (reader.Read())
                {
                    scoreboard += "\t"+reader[0];
                    scoreboard += "\t|\t";
                    scoreboard += reader[1]+"\n";
                }
                
                return scoreboard;
            }
            catch (Npgsql.PostgresException)
            {
                return null;
            }
        }

        public List<TradeCards> ShowAvailableTrades()
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("Select id, username, card_id, typ, element, damage from trade", conn);
            cmd.Prepare();


            try
            {
                // ReSharper disable once PossibleNullReferenceException
                var reader = cmd.ExecuteReader();

                List<TradeCards> trades = new List<TradeCards>();

                while (reader.Read())
                {
                    var trade = new TradeCards();
                    trade.Id = (string)reader[0];
                    trade.Username = (string)reader[1];
                    trade.CardId = (string)reader[2];
                    trade.RequirementType = (string)reader[3];
                    trade.RequirementElement = (string)reader[4];
                    trade.RequirementDamage = (double)((decimal)reader[5]);

                    trades.Add(trade);
                }
                
                return trades;
            }
            catch (Npgsql.PostgresException)
            {
                return null;
            }
        }

        public string CreateTrade(string username, TradeCards trade)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            //Check if card is owned
            var cmd = new NpgsqlCommand("Select * from player_cards where username = @username and card_id = @card_id", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("card_id", trade.CardId);
            cmd.Prepare();

            

            //Check if card is in deck
            var cmd2 = new NpgsqlCommand("Select * from player_deck where username = @username and card_id = @card_id", conn);
            cmd2.Parameters.AddWithValue("username", username);
            cmd2.Parameters.AddWithValue("card_id", trade.CardId);
            cmd2.Prepare();

            //Create Trade
            var cmd3 = new NpgsqlCommand("INSERT INTO trade (id, username, card_id, typ, element, damage) VALUES (@id, @username, @card_id, @typ, @element, @damage)", conn);
            cmd3.Parameters.AddWithValue("id", trade.Id);
            cmd3.Parameters.AddWithValue("username", username);
            cmd3.Parameters.AddWithValue("card_id", trade.CardId);
            if(trade.RequirementType != null)
                cmd3.Parameters.AddWithValue("typ", trade.RequirementType);
            else
                cmd3.Parameters.AddWithValue("typ", "");
            if(trade.RequirementElement != null)
                cmd3.Parameters.AddWithValue("element", trade.RequirementElement);
            else
                cmd3.Parameters.AddWithValue("element", "");
            cmd3.Parameters.AddWithValue("damage", trade.RequirementDamage);
            cmd3.Prepare();

            try
            {
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return "Card was not found in the users stack!";

                reader.Close();

                var reader2 = cmd2.ExecuteReader();
                if (reader2.HasRows)
                    return "Card is currently in a deck!";

                reader2.Close();

                cmd3.ExecuteNonQuery();
                return "Successfully created trade request!";
            }
            catch (Npgsql.PostgresException)
            {
                return "Internal database error!";
            }
        }

        public string DeleteTrade(string username, string tradeID)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            //check if trade exists
            var cmd = new NpgsqlCommand("Select * from trade where id = @id", conn);
            cmd.Parameters.AddWithValue("id", tradeID);
            cmd.Prepare();

            //check if user owns trade
            var cmd2 = new NpgsqlCommand("Select * from trade where username = @username and id = @id", conn);
            cmd2.Parameters.AddWithValue("username", username);
            cmd2.Parameters.AddWithValue("id", tradeID);
            cmd2.Prepare();

            //delete trade
            var cmd3 = new NpgsqlCommand("DELETE FROM trade where id = @id", conn);
            cmd3.Parameters.AddWithValue("id", tradeID);
            cmd3.Prepare();

            try
            {
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return "No corresponding trade request found!";

                reader.Close();

                var reader2 = cmd2.ExecuteReader();
                if (!reader2.HasRows)
                    return "Unauthorized command!";

                reader2.Close();

                cmd3.ExecuteNonQuery();
                return "Successfully deleted trade request!";
            }
            catch (Npgsql.PostgresException)
            {
                return "Internal database error!";
            }
        }

        public string ExecuteTrade(string username)
        {
            //Check if trade exists
            //Get trade data
            //Check the 2 trader (trading with oneself)
            //Check if card to be traded meets requirements
            //Delete trade request
            //Update player_cards information
            
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