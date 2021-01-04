using System;
using System.Collections.Generic;
using System.IO;

namespace MonsterTCG
{
    public class ContextManager:IContextManager
    {
        public Dictionary<string, string> Information { get; set; }
        public string Request { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Payload { get; set; }

        public ContextManager(StreamReader reader)
        {
            Information = new Dictionary<string, string>();
            string[] separator = {" "};
            var header = reader.ReadLine();

            Console.WriteLine("Received Request: \n\n" + header);

            if (header != null)
            {
                string[] words = header.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                Request = words[0];
                Path = words[1];
                Version = words[2];
                Information.Add("Request", Request);
                Information.Add("Path", Path);
                Information.Add("Version", Version);
            
                separator = new[]{": "};
                do
                {
                    header = reader.ReadLine();
                    if (header != null) words = header.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 2)
                    {
                        Information.Add(words[0], words[1]);
                        Console.WriteLine(words[0]+": "+words[1]);
                    }
                } while (header != "" && header != "\r\n");
            }

            if (Information.ContainsKey("Content-Length"))
            {
                if (Information.TryGetValue("Content-Length", out var contentLengthText))
                {
                    int contentLength = Int32.Parse(contentLengthText);
                    if (contentLength != 0)
                    {
                        string msg = "";
                        int temp;

                        for (int i = 0; i < contentLength; i++)
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
        }
    }
}
