using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_HTTP_Webservice
{
    class HttpPackage
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

        public HttpPackage(string message)
        {
            string[] separator = {" ", "Host: "};
            string[] words = message.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            Request = words[0];
            Path = words[1];
            Version = words[2];

            separator = new string[]{"Host: ", "User-Agent: "};
            words = message.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            Host = words[1];

            separator = new string[]{"User-Agent: ", "Accept: "};
            words = message.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            UserAgent = words[1];

            separator = new string[]{"Accept: ", "Content-Length: "};
            words = message.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            Accept = words[1];

            if (words.Length > 2)
            {
                separator = new string[]{"Content-Length: ", "Content-Type: "};
                words = message.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                ContentLength = Int32.Parse(words[1]);
                
                separator = new string[]{"Content-Type: "};
                words = message.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                ContentType = words[1];
            }
        }

        public string GetInfo()
        {
            return "Request: "+Request+"\nPath: "+Path+"\nVersion: "+Version+"\nHost: "+Host+"\nUserAgent: "+UserAgent+"\nAccept: "+Accept+"\nContentLength: "+ContentLength+"\nContentType: "+ContentType+"\n\nPayload: "+Payload+"\n\n";
        }
    }
}
