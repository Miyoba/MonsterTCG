using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_HTTP_Webservice
{
    public class RequestContext:IRequestContext
    {
        public Dictionary<string, string> Information { get; set; }
        public string Request { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        


        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string Payload { get; set; }

        public RequestContext()
        {
        }

        public RequestContext(StreamReader reader)
        {
            Information = new Dictionary<string, string>();
            string[] separator = {" "};
            var header = reader.ReadLine();

            Console.WriteLine("Received Request: \n\n" + header);

            string[] words = header.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

            Request = words[0];
            Path = words[1];
            Version = words[2];
            Information.Add("Request", Request);
            Information.Add("Path", Path);
            Information.Add("Version", Version);
            
            separator = new string[]{": "};
            do
            {
                header = reader.ReadLine();
                words = header.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 2)
                {
                    Information.Add(words[0], words[1]);
                    Console.WriteLine(words[0]+": "+words[1]);
                }
            } while (header != "" && header != "\r\n");

            foreach(KeyValuePair<string, string> entry in Information) 
            {
                switch (entry.Key)
                {
                        case "Host":
                            Host = entry.Value;
                            break;
                        case "User-Agent":
                            UserAgent = entry.Value;
                            break;
                        case "Accept":
                            Accept = entry.Value;
                            break;
                        case "Content-Length":
                            ContentLength = Int32.Parse(entry.Value);
                            break;
                        case "Content-Type":
                            ContentType = entry.Value;
                            break;
                }
            }

            if (Information.ContainsKey("Content-Length"))
            {
                if (ContentLength != 0)
                {
                    string msg = "";
                    int temp;

                    for (int i = 0; i < ContentLength; i++)
                    {
                        temp = reader.Read();
                        if (temp == -1)
                            break;
                        msg += (char) temp;
                    }
                    Payload = msg;
                }
            }
        }

        public string GetOk(string response)
        {
            Console.WriteLine("OK Response: \r\n"+response+"\r\n");
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
            Console.WriteLine("CREATED Response: \r\n"+response+"\r\n");
            var erg = "";
            erg += Version + " 201 Created\r\n"+
                   "Server: MyServer\r\n"+
                   "Content-Type: text/plain\r\n"+
                   "Content-Length: "+response.Length+"\r\n"+
                   "\r\n"+
                   response;
            return erg;
        }

        public string GetNoContent(string response)
        {
            Console.WriteLine("NO CONTENT Response: \r\n"+response+"\r\n");
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
            Console.WriteLine("BAD REQUEST Response: \r\n"+response+"\r\n");
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
            Console.WriteLine("NOT FOUND Response: \r\n"+response+"\r\n");
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
