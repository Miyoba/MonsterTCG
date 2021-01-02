using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            catch (Newtonsoft.Json.JsonSerializationException)
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
            catch (Newtonsoft.Json.JsonSerializationException)
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
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                return new TextResponse(StatusCodesEnum.InternalServerError, "Could not deserialize json data!");
            }
        }

        public IResponse ShowUserProfile()
        {
            throw new System.NotImplementedException();
        }
        public IResponse EditUserProfile()
        {
            throw new System.NotImplementedException();
        }

        public IResponse ShowStats()
        {
            throw new System.NotImplementedException();
        }

        public IResponse ShowScoreboard()
        {
            throw new System.NotImplementedException();
        }

        public IResponse CreateTrade()
        {
            throw new System.NotImplementedException();
        }

        public IResponse ShowTrades()
        {
            throw new System.NotImplementedException();
        }

        public IResponse DeleteTrade()
        {
            throw new System.NotImplementedException();
        }

        public IResponse ExecuteTrade()
        {
            throw new System.NotImplementedException();
        }

        public IResponse BeginBattle()
        {
            throw new System.NotImplementedException();
        }
    }
}
