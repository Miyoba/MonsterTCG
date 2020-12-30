using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public interface IContextManager
    {
        public Dictionary<string, string> Information { get; set; }
        public string Request { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Payload { get; set; }

    }
}