using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Npgsql;

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
            catch (PostgresException)
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
            catch (PostgresException)
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
            catch (PostgresException)
            {
                return false;
            }
        }

        public string BuyPackage(string token)
        {

            string erg;
            int coins;
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
            catch (PostgresException)
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

            catch (PostgresException)
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
                catch (PostgresException)
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
            catch (PostgresException)
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
            catch (PostgresException)
            {
                return null;
            }
        }

        public string ConfigureDeck(string username, List<string> cards)
        {
            
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

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
            catch (PostgresException)
            {
                return null;
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
            catch (PostgresException)
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
            catch (PostgresException)
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
            catch (PostgresException)
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
            catch (PostgresException)
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
            catch (PostgresException)
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
            catch (PostgresException)
            {
                return "Internal database error!";
            }
        }

        public string DeleteTrade(string username, string tradeId)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            //check if trade exists
            var cmd = new NpgsqlCommand("Select * from trade where id = @id", conn);
            cmd.Parameters.AddWithValue("id", tradeId);
            cmd.Prepare();

            //check if user owns trade
            var cmd2 = new NpgsqlCommand("Select * from trade where username = @username and id = @id", conn);
            cmd2.Parameters.AddWithValue("username", username);
            cmd2.Parameters.AddWithValue("id", tradeId);
            cmd2.Prepare();

            //delete trade
            var cmd3 = new NpgsqlCommand("DELETE FROM trade where id = @id", conn);
            cmd3.Parameters.AddWithValue("id", tradeId);
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
            catch (PostgresException)
            {
                return "Internal database error!";
            }
        }

        public int ExecuteTrade(string username, string tradeId, string cardId)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var tradeCardJson = new JsonCard();
            var tradeRequest = new TradeCards();

            //Check if trade exists
            var cmd = new NpgsqlCommand("Select * from trade where id = @id", conn);
            cmd.Parameters.AddWithValue("id", tradeId);
            cmd.Prepare();

            //Check if user owns trade
            var cmd2 = new NpgsqlCommand("Select * from trade where username = @username and id = @id", conn);
            cmd2.Parameters.AddWithValue("username", username);
            cmd2.Parameters.AddWithValue("id", tradeId);
            cmd2.Prepare();

            //Check if user owns card and get card data
            var cmd3 = new NpgsqlCommand("Select id, name, damage from player_cards inner join cards on player_cards.card_id = cards.id where username = @username and card_id = @card_id", conn);
            cmd3.Parameters.AddWithValue("username", username);
            cmd3.Parameters.AddWithValue("card_id", cardId);
            cmd3.Prepare();

            //Get trade data
            var cmd4 = new NpgsqlCommand("Select id, username, card_id, typ, element, damage from trade where id = @id", conn);
            cmd4.Parameters.AddWithValue("id", tradeId);
            cmd4.Prepare();


            //Check if card to be traded meets requirements

            //Delete trade request
            var cmd5 = new NpgsqlCommand("DELETE FROM trade where card_id = @card_id", conn);
            

            //Update player_cards information (needs card_id from tradeRequest)
            var cmd6 = new NpgsqlCommand("UPDATE player_cards SET username = @username where card_id = @card_id", conn);
            cmd6.Parameters.AddWithValue("username", username);
            

            //Update second player_cards information (needs username from tradeRequest)
            var cmd7 = new NpgsqlCommand("UPDATE player_cards SET username = @username where card_id = @card_id", conn);
            cmd7.Parameters.AddWithValue("card_id", cardId);

            try
            {
                //check if Trade request exists
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return -2;

                reader.Close();

                //Check if user owns trade
                var reader2 = cmd2.ExecuteReader();
                if (reader2.HasRows)
                    return -3;

                reader2.Close();

                //Check if user owns card and get card data
                var reader3 = cmd3.ExecuteReader();
                if (!reader3.HasRows)
                    return -4;
                while (reader3.Read())
                {
                    tradeCardJson.Id = (string) reader3[0];
                    tradeCardJson.Name = (string) reader3[1];
                    tradeCardJson.Damage = (double) ((decimal) reader3[2]);
                }

                var tradeCard = tradeCardJson.ConvertToCard();

                reader3.Close();

                var reader4 = cmd4.ExecuteReader();
                while (reader4.Read())
                {
                    //id, username, card_id, typ, element, damage
                    tradeRequest.Id = (string) reader4[0];
                    tradeRequest.Username = (string) reader4[1];
                    tradeRequest.CardId = (string) reader4[2];
                    tradeRequest.RequirementType = (string) reader4[3];
                    tradeRequest.RequirementElement = (string) reader4[4];
                    tradeRequest.RequirementDamage = (double)((decimal) reader4[5]);
                }

                reader4.Close();

                //Check if card to be traded meets requirements

                if (tradeCard.Damage < tradeRequest.RequirementDamage)
                    return -10;

                if (!tradeRequest.RequirementType.Equals(""))
                {
                    if (tradeRequest.RequirementType.ToLower().Equals("monster") && tradeCard is SpellCard)
                        return -11;
                    if (tradeRequest.RequirementType.ToLower().Equals("spell") && tradeCard.GetType().IsSubclassOf(typeof(MonsterCard)))
                        return -11;
                }
                if (!tradeRequest.RequirementElement.Equals(""))
                {
                    if (tradeRequest.RequirementElement.ToLower().Equals("fire") && !(tradeCard.Element is Fire))
                        return -12;
                    if (tradeRequest.RequirementElement.ToLower().Equals("water") && !(tradeCard.Element is Water))
                        return -12;
                    if ((tradeRequest.RequirementElement.ToLower().Equals("normal") || tradeRequest.RequirementElement.ToLower().Equals("regular")) && !(tradeCard.Element is Normal))
                        return -12;
                }


                cmd5.Parameters.AddWithValue("card_id", tradeRequest.CardId);
                cmd5.Prepare();

                if (cmd5.ExecuteNonQuery() == -1)
                    return -13;

                cmd6.Parameters.AddWithValue("card_id", tradeRequest.CardId);
                cmd6.Prepare();

                if (cmd6.ExecuteNonQuery() == -1)
                    return -14;

                cmd7.Parameters.AddWithValue("username", tradeRequest.Username);
                cmd7.Prepare();

                if (cmd7.ExecuteNonQuery() == -1)
                    return -14;

                return 1;
            }
            catch (PostgresException)
            {
                return -1;
            }
        }

        public string ExecuteBattle(string username)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            //Check if user has deck
            var cmd = new NpgsqlCommand("Select * from player_deck where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            //Check if Battle entry without user exists
            var cmd2 = new NpgsqlCommand("Select id from battle where player1 != @username and player2 is null", conn);
            cmd2.Parameters.AddWithValue("username", username);
            cmd2.Prepare();

            //Create Battle entry if necessary
            var cmd3 = new NpgsqlCommand("INSERT INTO battle (player1) VALUES (@username)", conn);
            cmd3.Parameters.AddWithValue("username", username);
            cmd3.Prepare();

            var cmdUpdate = new NpgsqlCommand("UPDATE battle SET player2 = @username where id = @id", conn);

            //Get created BattleID
            var cmdBattle = new NpgsqlCommand("Select id from battle where player1 = @username", conn);
            cmdBattle.Parameters.AddWithValue("username", username);
            cmdBattle.Prepare();

            //If Battle entry exists already execute battle
            var cmd4 = new NpgsqlCommand("Select player1 from battle where id = @id", conn);
            var cmd5 = new NpgsqlCommand("Select id, name, damage from cards inner join player_deck on cards.id = player_deck.card_id where username = @username", conn);
            var cmd6 = new NpgsqlCommand("Select id, name, damage from cards inner join player_deck on cards.id = player_deck.card_id where username = @username", conn);

            //reduce Elo
            var cmdLoser = new NpgsqlCommand("UPDATE player SET elo = (elo-5) where username = @username", conn);
            var cmdWinner = new NpgsqlCommand("UPDATE player SET elo = (elo+3) where username = @username", conn);

            //Save Battle in BattleLog
            var cmd9 = new NpgsqlCommand("INSERT INTO battle_log (id, battle_text, winner) VALUES (@id, @battle_text, @winner)", conn);

            //Loop until BattleLog exists
            var cmd10 = new NpgsqlCommand("Select battle_text from battle_log where id = @id", conn);

            try
            {
                //Check if user has deck
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return "No deck found!";

                reader.Close();

                //Check if Battle entry without user exists
                var reader2 = cmd2.ExecuteReader();
                //If Battle entry exists already -> execute battle
                int battleId;
                if (reader2.HasRows)
                {
                    reader2.Read();
                    battleId = (int)reader2[0];
                    reader2.Close();

                    cmd4.Parameters.AddWithValue("id", battleId);
                    cmd4.Prepare();

                    var reader4 = cmd4.ExecuteReader();
                    if (!reader4.HasRows)
                        return null;
                    reader4.Read();
                    var player1 = (string)reader4[0];
                    var player2 = username;

                    reader4.Close();

                    cmdUpdate.Parameters.AddWithValue("username", username);
                    cmdUpdate.Parameters.AddWithValue("id", battleId);
                    cmdUpdate.Prepare();

                    cmdUpdate.ExecuteNonQuery();

                    cmd5.Parameters.AddWithValue("username", player1);
                    cmd5.Prepare();

                    cmd6.Parameters.AddWithValue("username", player2);
                    cmd6.Prepare();

                    var cardList1 = new List<ICard>();
                    var cardList2 = new List<ICard>();

                    var reader5 = cmd5.ExecuteReader();
                    if (!reader5.HasRows)
                        return null;
                    while (reader5.Read())
                    {
                        var card = new JsonCard();
                        card.Id = (string)reader5[0];
                        card.Name = (string) reader5[1];
                        card.Damage = (double) ((decimal) reader5[2]);
                        cardList1.Add(card.ConvertToCard());
                    }

                    reader5.Close();

                    var reader6 = cmd6.ExecuteReader();
                    if (!reader6.HasRows)
                        return null;
                    while (reader6.Read())
                    {
                        var card = new JsonCard();
                        card.Id = (string)reader6[0];
                        card.Name = (string) reader6[1];
                        card.Damage = (double) ((decimal) reader6[2]);
                        cardList2.Add(card.ConvertToCard());
                    }

                    reader6.Close();


                    //DB inputs
                    var deck1 = new CardDeck();
                    var deck2 = new CardDeck();
                    var users = new List<User>();
                    var user1 = new User();
                    var user2 = new User();

                    deck1.Cards = cardList1;
                    deck2.Cards = cardList2;
                    user1.SelectedDeck = deck1;
                    user2.SelectedDeck = deck2;
                    user1.Username = player1;
                    user2.Username = player2;
                    users.Add(user1);
                    users.Add(user2);

                    Battle battle = new Battle(users);
                    var log = battle.Fight();

                    //reduce elo
                    if (log.Winner != null)
                    {
                        cmdWinner.Parameters.AddWithValue("username", log.Winner.Username);
                        cmdWinner.Prepare();

                        cmdLoser.Parameters.AddWithValue("username",
                            player1.Equals(log.Winner.Username) ? player2 : player1);
                        cmdLoser.Prepare();

                        cmdLoser.ExecuteNonQuery();
                        cmdWinner.ExecuteNonQuery();
                    }

                    //Save Battle in BattleLog
                    cmd9.Parameters.AddWithValue("id", battleId);
                    cmd9.Parameters.AddWithValue("battle_text", log.Log);
                    if(log.Winner != null)
                        cmd9.Parameters.AddWithValue("winner", log.Winner.Username);
                    else
                        cmd9.Parameters.AddWithValue("winner", "");
                    cmd9.Prepare();
                    cmd9.ExecuteNonQuery();

                }

                //Create Battle entry if necessary
                else
                {
                    reader2.Close();
                    
                    var reader3 = cmd3.ExecuteNonQuery();
                    if (reader3 == -1)
                        return null;

                    //Get created battleID
                    // ReSharper disable once PossibleNullReferenceException
                    battleId = (int)cmdBattle.ExecuteScalar();
                }


                //Loop until BattleLog exists

                cmd10.Parameters.AddWithValue("id", battleId);
                cmd10.Prepare();

                while(true)
                {
                    var reader10 = cmd10.ExecuteReader();
                    if (reader10.HasRows)
                    {
                        reader10.Read();
                        return (string)reader10[0];
                    }
                    reader10.Close();

                    System.Threading.Thread.Sleep(1000);
                }

            }
            catch (PostgresException e)
            {
                Console.WriteLine(e.MessageText);
                return null;
            }
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
            catch (PostgresException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}