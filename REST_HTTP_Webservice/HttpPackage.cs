using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_HTTP_Webservice
{
    public class HttpPackage
    {
        public string Request { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string Payload { get; set; }

        public HttpPackage(List<string> messages)
        {
            Request = null;
            Path = null;
            Version = null;
            ContentLength = 0;

            foreach (var message in messages)
            {
                string[] separator = {" "};
                string[] words = message.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

                if (words.Length == 3)
                {
                    Request = words[0];
                    Path = words[1];
                    Version = words[2];
                }
                else if(words.Length != 0)
                {
                    switch (words[0])
                    {
                        case "Host:":
                            Host = words[1];
                            break;
                        case "User-Agent:":
                            UserAgent = words[1];
                            break;
                        case "Accept:":
                            Accept = words[1];
                            break;
                        case "Content-Length:":
                            ContentLength = Int32.Parse(words[1]);
                            break;
                        case "Content-Type:":
                            ContentType = words[1];
                            break;

                    }
                }
            }
        }

        public Dictionary<string, string> GetInfo()
        {
            var erg = new Dictionary<string, string>();
            erg.Add("Request",Request);
            erg.Add("Path",Path);
            erg.Add("Version",Version);
            erg.Add("Host",Host);
            erg.Add("UserAgent",UserAgent);
            erg.Add("Accept",Accept);
            erg.Add("ContentLength",""+ContentLength);
            erg.Add("ContentType",ContentType);
            erg.Add("Payload",Payload);
            return erg;
        }

        public string GetOk(string response)
        {
            Console.WriteLine("OK Response: "+response+"\r\n");
            var erg = "";
            erg += Version + " 200 OK\r\n"+
                   "Server: MyServer\r\n"+
                   "Content-Type: text/plain\r\n"+
                   "Content-Length: "+response.Length+"\r\n"+
                   "\r\n"+
                   response;
            return erg;
        }

        public string GetCreated(string response)
        {
            Console.WriteLine("CREATED Response: "+response+"\r\n");
            var erg = "";
            erg += Version + " 201 Created\r\n"+
                   "Server: MyServer\r\n"+
                   "Content-Type: text/plain;\r\n"+
                   "Content-Length: "+response.Length+"\r\n"+
                   "\r\n"+
                   response;
            return erg;
        }

        public string GetNoContent(string response)
        {
            Console.WriteLine("NO CONTENT Response: "+response+"\r\n");
            var erg = "";
            erg += Version + " 204 No Content\r\n"+
                   "Server: MyServer\r\n"+
                   "Content-Type: text/plain\r\n"+
                   "Content-Length: "+response.Length+"\r\n"+
                   "\r\n"+
                   response;
            return erg;
        }

        public string GetBadRequest(string response)
        {
            Console.WriteLine("BAD REQUEST Response: "+response+"\r\n");
            var erg = "";
            erg += Version + " 400 Bad Request\r\n"+
                   "Server: MyServer\r\n"+
                   "Content-Type: text/plain\r\n"+
                   "Content-Length: "+response.Length+"\r\n"+
                   "\r\n"+
                   response;
            return erg;
        }
        public string GetNotFound(string response)
        {
            Console.WriteLine("NOT FOUND Response: "+response+"\r\n");
            var erg = "";
            erg += Version + " 404 Not Found\r\n"+
                   "Server: MyServer\r\n"+
                   "Content-Type: text/plain\r\n"+
                   "Content-Length: "+response.Length+"\r\n"+
                   "\r\n"+
                   response;
            return erg;
        }

        public bool CheckRequest()
        {
            if (Version != null && Request != null && Path != null)
            {
                if (string.Equals(Request, "GET") || string.Equals(Request, "POST") || string.Equals(Request, "PUT") ||
                    string.Equals(Request, "DELETE"))
                {
                    string[] separator = {"/"};
                    string[] words = Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length > 0 && words.Length < 3)
                    {
                        if (string.Equals(words[0], "messages"))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }
}
