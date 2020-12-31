using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class ResponseManager:IResponseManager
    {
        public IResponse Response { get; set; }
        private readonly string _serverName;
        public ResponseManager(IResponse response, string serverName)
        {
            Response = response;
            _serverName = serverName;
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
                case StatusCodesEnum.Conflict:
                    return GetConflict();
                case StatusCodesEnum.PaymentRequired:
                    return GetPaymentRequired();
                case StatusCodesEnum.Forbidden:
                    return GetForbidden();
                case StatusCodesEnum.InternalServerError:
                    return GetInternalServerError();
                default:
                    return GetBadRequest();
            }
        }

        private string GetOk()
        {
            Console.WriteLine("OK Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 200 OK\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        private string GetCreated()
        {
            Console.WriteLine("CREATED Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 201 Created\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        private string GetNoContent()
        {
            Console.WriteLine("NO CONTENT Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 204 No Content\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        private string GetBadRequest()
        {
            Console.WriteLine("BAD REQUEST Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 400 Bad Request\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }
        private string GetNotFound()
        {
            Console.WriteLine("NOT FOUND Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 404 Not Found\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        private string GetConflict()
        {
            Console.WriteLine("CONFLICT Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 409 Conflict\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        private string GetPaymentRequired()
        {
            Console.WriteLine("PAYMENT REQUIRED Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 402 Payment Required\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        private string GetForbidden()
        {
            Console.WriteLine("FORBIDDEN Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 403 Forbidden\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }

        private string GetInternalServerError()
        {
            Console.WriteLine("INTERNAL SERVER ERROR Response\r\n");
            var erg = "";
            erg += "HTTP/1.1 500 Internal Server Error\r\n"+
                   "Server: "+_serverName+"\r\n"+
                   "Content-Type: "+Response.ContentType+"\r\n"+
                   "Content-Length: "+Response.Content.Length+"\r\n"+
                   "\r\n"+
                   Response.Content;
            return erg;
        }
    }
}