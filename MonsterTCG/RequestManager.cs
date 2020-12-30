using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{
    public class RequestManager
    {
        public IContextManager Context { get; set; }
        public MonsterTcgdb Db { get; set; }

        public RequestManager(IContextManager context)
        {
            Context = context;
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
            {
                var dir = directoryInfo.FullName + "\\messages";
                // If directory does not exist, create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
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
                                return ShowDeck();
                            case "deck?format=plain":
                                return ShowDeck();
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

            throw new System.NotImplementedException();
        }

        public IResponse CreateUser()
        {
            throw new System.NotImplementedException();
        }

        public IResponse LoginUser()
        {
            throw new System.NotImplementedException();
        }

        public IResponse CreatePackageStacked()
        {
            //Done by the admin
            throw new System.NotImplementedException();
        }

        public IResponse BuyPackage()
        {
            throw new System.NotImplementedException();
        }

        public IResponse CreatePackageRandom()
        {
            //Done by the admin
            throw new System.NotImplementedException();
        }

        public IResponse ShowOwnedCards()
        {
            throw new System.NotImplementedException();
        }

        public IResponse ShowDeck()
        {
            throw new System.NotImplementedException();
        }

        public IResponse ConfigureDeck()
        {
            //Has to find max. number of cards in deck
            throw new System.NotImplementedException();
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
