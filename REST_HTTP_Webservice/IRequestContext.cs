using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace REST_HTTP_Webservice
{
    public interface IRequestContext
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

        public string GetOk(string response);
        public string GetCreated(string response);
        public string GetNoContent(string response);
        public string GetBadRequest(string response);
        public string GetNotFound(string response);
        public bool CheckRequest();
    }
}