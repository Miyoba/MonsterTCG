using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class ResponseManager:IResponseManager
    {
        public IResponse Response { get; set; }
        public ResponseManager(IResponse response)
        {

        }

        public string ProcessResponse()
        {
            switch (Response.Status)
            {
                case StatusCodesEnum.BadRequest:
                    return GetBadRequest();
                case StatusCodesEnum.Ok:
                    return GetOk();
                case StatusCodesEnum.Created:
                    return GetCreated();
                case StatusCodesEnum.NoContent:
                    return GetNoContent();
                case StatusCodesEnum.NotFound:
                    return GetNotFound();
                default:
                    return GetBadRequest();
            }
        }

        public string GetOk()
        {
            Console.WriteLine("OK Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 200 OK\r\n"+
                   "Server: MonsterTCG-Server\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        public string GetCreated()
        {
            Console.WriteLine("CREATED Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 201 Created\r\n"+
                   "Server: MonsterTCG-Server\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        public string GetNoContent()
        {
            Console.WriteLine("NO CONTENT Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 204 No Content\r\n"+
                   "Server: MonsterTCG-Server\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        public string GetBadRequest()
        {
            Console.WriteLine("BAD REQUEST Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 400 Bad Request\r\n"+
                   "Server: MonsterTCG-Server\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }
        public string GetNotFound()
        {
            Console.WriteLine("NOT FOUND Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 404 Not Found\r\n"+
                   "Server: MonsterTCG-Server\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }
    }
}