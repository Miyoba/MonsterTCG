using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MonsterTCG
{
    public class RequestManager
    {
        public IContextManager Context { get; set; }
        public MonsterTcgdb Db { get; set; }

        public RequestManager(IContextManager context)
        {
            Context = context;
            Db = new MonsterTcgdb();
        }

        public IResponse ProcessRequest()
        {
            string[] separator = {"/"};
            string[] words = Context.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            if (words.Length >= 1)
            {
                switch (Context.Request.ToUpper())
                {
                    case "GET":
                        switch (words[0].ToLower())
                        {
                            case "cards":
                                return ShowOwnedCards();
                            case "deck":
                                return ShowDeck(new JsonResponse(StatusCodesEnum.Ok, ""));
                            case "deck?format=plain":
                                return ShowDeck(new TextResponse(StatusCodesEnum.Ok, ""));
                            case "users":
                                if (words.Length == 2)
                                    return ShowUserProfile();
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                            case "stats":
                                return ShowStats();
                            case "score":
                                return ShowScoreboard();
                            case "tradings":
                                return ShowTrades();
                            default:
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                        }
                    case "POST":
                        switch (words[0].ToLower())
                        {
                            case "users":
                                return CreateUser();
                            case "sessions":
                                return LoginUser();
                            case "packages":
                                return CreatePackageStacked();
                            case "packagesRandom":
                                return CreatePackageRandom();
                            case "transactions":
                                return BuyPackage();
                            case "battles":
                                return BeginBattle();
                            case "tradings":
                                if(words.Length == 1)
                                    return CreateTrade();
                                if (words.Length == 2)
                                    return ExecuteTrade();
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                            default:
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                        }
                    case "PUT":
                        switch (words[0].ToLower())
                        {
                            case "deck":
                                return ConfigureDeck();
                            case "users":
                                if (words.Length == 2)
                                    return EditUserProfile();
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                            default:
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                        }
                    case "DELETE":
                        switch (words[0].ToLower())
                        {
                            case "tradings":
                                if (words.Length == 2)
                                    return DeleteTrade();
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                            default:
                                return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
                        }
                    default:
                        return new TextResponse(StatusCodesEnum.BadRequest, "Unknown Request!");
                }
            }
            return new TextResponse(StatusCodesEnum.BadRequest, "Invalid Command!");
        }

        public IResponse CreateUser()
        {
            try
            {
                User user = JsonConvert.DeserializeObject<User>(Context.Payload);
                bool erg = Db.RegisterUser(user.Username, user.Password);

                if (erg)
                    return new TextResponse(StatusCodesEnum.Created, "User successfully created!");
                return new TextResponse(StatusCodesEnum.Conflict, "Username already taken. User could not be created!");
            }
            catch (JsonSerializationException)
            {
                return new TextResponse(StatusCodesEnum.InternalServerError, "Could not deserialize json data!");
            }
        }

        public IResponse LoginUser()
        {
            try
            {
                User user = JsonConvert.DeserializeObject<User>(Context.Payload);
                string token = Db.LoginUser(user.Username, user.Password);
            
                if(token is null)
                    return new TextResponse(StatusCodesEnum.NotFound,"User or password is incorrect!");
                return new TextResponse(StatusCodesEnum.Ok,token);
            }
            catch (JsonSerializationException)
            {
                return new TextResponse(StatusCodesEnum.InternalServerError, "Could not deserialize json data!");
            }

            
        }

        public IResponse CreatePackageStacked()
        {
            //Done by the admin
            string token;
            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            if(!token.Equals("Basic admin-mtcgToken"))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            if(Db.CreatePackage(Context.Payload))
                return new TextResponse(StatusCodesEnum.Created,"Package successfully created!"); 
            return new TextResponse(StatusCodesEnum.InternalServerError, "Database couldn't save package!");
        }

        public IResponse BuyPackage()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            if(Db.GetUsernameFromToken(token) == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            var jsonCards = Db.BuyPackage(token);
            if (jsonCards != null)
                return new JsonResponse(StatusCodesEnum.Ok, jsonCards);
            return new TextResponse(StatusCodesEnum.PaymentRequired, "Not enough coins or packages left!");
        }

        public IResponse CreatePackageRandom()
        {
            //Done by the admin
            throw new System.NotImplementedException();
        }

        public IResponse ShowOwnedCards()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            if(Db.GetUsernameFromToken(token) == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            var jsonCards = Db.ShowCards(token);
            if (jsonCards != null)
                return new JsonResponse(StatusCodesEnum.Ok, JsonConvert.SerializeObject(jsonCards));
            return new TextResponse(StatusCodesEnum.NotFound, "No cards found!");
        }

        public IResponse ShowDeck(IResponse type)
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");
            var deck = Db.ShowDeck(username);

            if (deck != null)
            {
                if (type is JsonResponse)
                    return new JsonResponse(StatusCodesEnum.Ok, JsonConvert.SerializeObject(deck));
                return new JsonResponse(StatusCodesEnum.Ok, string.Join( "\n", deck));
            }

            return new TextResponse(StatusCodesEnum.NotFound, "No cards in deck found!");
        }

        public IResponse ConfigureDeck()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");


            try
            {
                List<string> cards = JsonConvert.DeserializeObject<List<string>>(Context.Payload);
                if(cards.Count != 4)
                    return new TextResponse(StatusCodesEnum.Forbidden, "A deck is only allowed to contain exactly 4 cards!");

                var check = cards.Distinct().ToList();

                if(check.Count != 4)
                    return new TextResponse(StatusCodesEnum.Forbidden, "You cannot add multiple copies of the exact same card into your deck!");

                string erg = Db.ConfigureDeck(username, cards);

                if (erg == null)
                    return new TextResponse(StatusCodesEnum.Created, "Deck successfully configured!");
                if(erg.Equals("Internal database error!"))
                    return new TextResponse(StatusCodesEnum.InternalServerError, erg);
                return new TextResponse(StatusCodesEnum.Forbidden, erg);
            }
            catch (JsonSerializationException)
            {
                return new TextResponse(StatusCodesEnum.InternalServerError, "Could not deserialize json data!");
            }
        }

        public IResponse ShowUserProfile()
        {

            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string[] separator = {"/"};
            string[] words = Context.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

            if(!words[1].Equals(username))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");


            var erg = Db.ShowPlayerData(username);
            if(erg != null)
                return new JsonResponse(StatusCodesEnum.Ok, JsonConvert.SerializeObject(erg));
            return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
        }
        public IResponse EditUserProfile()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string[] separator = {"/"};
            string[] words = Context.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

            if(!words[1].Equals(username))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            var user = JsonConvert.DeserializeObject<JsonUser>(Context.Payload);
            user.Username = username;

            var erg = Db.EditPlayer(user);
            if(erg)
                return new TextResponse(StatusCodesEnum.Ok, "User information successfully changed!");
            return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
        }

        public IResponse ShowStats()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            var erg = Db.ShowStats(username);
            if(erg != null)
                return new TextResponse(StatusCodesEnum.Ok, erg);
            return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
        }

        public IResponse ShowScoreboard()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            var erg = Db.ShowScoreboard();
            if(erg != null)
                return new TextResponse(StatusCodesEnum.Ok, erg);
            return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
        }

        public IResponse ShowTrades()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            List<TradeCards> erg = Db.ShowAvailableTrades();


            if(erg == null)
                return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
            if(erg.Count == 0)
                return new TextResponse(StatusCodesEnum.NoContent, "No active trade requests found!");
            return new JsonResponse(StatusCodesEnum.Ok, JsonConvert.SerializeObject(erg));

        }

        public IResponse CreateTrade()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            TradeCards trade = JsonConvert.DeserializeObject<TradeCards>(Context.Payload);

            string erg = Db.CreateTrade(username, trade);
            if(erg == null)
                return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
            if(erg.Equals("Card was not found in the users stack!"))
                return new TextResponse(StatusCodesEnum.NotFound, erg);
            if(erg.Equals("Card is currently in a deck!"))
                return new TextResponse(StatusCodesEnum.Forbidden, erg);
            return new TextResponse(StatusCodesEnum.Created, erg);
            
        }

        public IResponse DeleteTrade()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string[] separator = {"/"};
            string[] words = Context.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

            var erg = Db.DeleteTrade(username, words[1]);
            if(erg == null)
                return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
            if(erg.Equals("No corresponding trade request found!"))
                return new TextResponse(StatusCodesEnum.NotFound, erg);
            if(erg.Equals("Unauthorized command!"))
                return new TextResponse(StatusCodesEnum.Unauthorized, erg);
            return new TextResponse(StatusCodesEnum.Ok, erg);

        }

        public IResponse ExecuteTrade()
        {
            string token;

            if(!(Context.Information.TryGetValue("Authorization",out token)))
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string username = Db.GetUsernameFromToken(token);

            if(username == null)
                return new TextResponse(StatusCodesEnum.Unauthorized,"Unauthorized command!");

            string[] separator = {"/"};
            string[] words = Context.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

            string cardId = JsonConvert.DeserializeObject<string>(Context.Payload);

            var erg = Db.ExecuteTrade(username, words[1], cardId);

            switch (erg)
            {
                case 1:
                    return new TextResponse(StatusCodesEnum.Ok, "Trade request successfully executed!");
                case -1:
                    return new TextResponse(StatusCodesEnum.InternalServerError, "Internal database error!");
                case -2:
                    return new TextResponse(StatusCodesEnum.NotFound, "Trade request could not be found!");
                case -3:
                    return new TextResponse(StatusCodesEnum.Forbidden, "Cannot execute the trade request!\nA trade has to involve 2 different user!");
                case -4:
                    return new TextResponse(StatusCodesEnum.NotFound, "Card could not be found!");
                case -10:
                    return new TextResponse(StatusCodesEnum.Conflict, "Card damage does not meet the requirements!");
                case -11:
                    return new TextResponse(StatusCodesEnum.Conflict, "Card type does not meet the requirements!");
                case -12:
                    return new TextResponse(StatusCodesEnum.Conflict, "Card element does not meet the requirements!");
                case -13:
                    return new TextResponse(StatusCodesEnum.InternalServerError, "Trade could not be deleted!");
                case -14:
                    return new TextResponse(StatusCodesEnum.InternalServerError, "Card could not be assigned to player!");

                default:
                    return new TextResponse(StatusCodesEnum.InternalServerError, "Unknown error!");
            }
        }

        public IResponse BeginBattle()
        {
            var card1 = new Knight("Id token", "BubbleKnight", 15.0, EnumElementType.Water);
            var card2 = new Knight("Id token", "FlameKnight", 25.0, EnumElementType.Fire);
            var card3 = new SpellCard("Id token", "FireSpell", 25.0, EnumElementType.Fire);
            var card4 = new SpellCard("Id token", "RegularSpell", 30.0, EnumElementType.Normal);
            var card5 = new SpellCard("Id token", "WaterSpell", 13.0, EnumElementType.Water);
            var card6 = new Kraken("Id token", "TinyOctopus", 5.0, EnumElementType.Normal);
            var card7 = new Goblin("Id token", "Goblin", 12.0, EnumElementType.Fire);
            var card8 = new Dragon("Id token", "Drogo", 18.0, EnumElementType.Normal);

            var deck1 = new CardDeck();
            deck1.Cards.Add(card1);
            deck1.Cards.Add(card3);
            deck1.Cards.Add(card5);
            deck1.Cards.Add(card7);

            var deck2 = new CardDeck();
            deck2.Cards.Add(card2);
            deck2.Cards.Add(card4);
            deck2.Cards.Add(card6);
            deck2.Cards.Add(card8);

            var user1 = new User();
            user1.Username = "Daniel";
            user1.SelectedDeck = deck1;

            var user2 = new User();
            user2.Username = "Wolfgang";
            user2.SelectedDeck = deck2;

            var list = new List<User>();
            list.Add(user1);
            list.Add(user2);

            var battle = new Battle(list);
            var log = battle.Fight();

            return new TextResponse(StatusCodesEnum.Ok, log.Log);
        }
    }
}
