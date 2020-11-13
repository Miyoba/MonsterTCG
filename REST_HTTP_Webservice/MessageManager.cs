using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace REST_HTTP_Webservice
{
    public class MessageManager
    {
        public HttpPackage Package { get; set; }
        public MessageManager(HttpPackage package)
        {
            Package = package;
        }

        public string ReadMessage()
        {
            // Open the stream and read it back.    
            using (StreamReader sr = File.OpenText(GetMessageDirPath()))    
            {    
                string s = "";    
                while ((s = sr.ReadLine()) != null)    
                {    
                    Console.WriteLine(s);    
                }    
            }    
            throw new NotImplementedException();
        }

        public string GetMessageDirPath()
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
                return directoryInfo.FullName + Package.Path;
            return null;
        }

        public string ListMessages()
        {
            throw new NotImplementedException();
        }

        public int GetNewMessageId(string current)
        {
            int messageId = 0;
            foreach (string file in Directory.EnumerateFiles(current))
            {
                string[] separator = {current+"\\"};
                string[] words = file.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine(words[0]);
                if (Int32.Parse(words[0]) > messageId)
                {
                    messageId = Int32.Parse(words[0]);
                }
            }

            return ++messageId;
        }

        public string SaveMessage()
        {


            var current = GetMessageDirPath();
            var messageId = GetNewMessageId(current);

            string fileName = current+"\\"+messageId;
        
            try    
            {    
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))    
                {    
                    File.Delete(fileName);    
                }    
        
                // Create a new file     
                using (FileStream fs = File.Create(fileName))     
                {    
                    // Add some text to file  
                    if (Package.ContentLength != 0)
                    {
                        Byte[] payload = new UTF8Encoding(true).GetBytes(Package.Payload);
                        fs.Write(payload, 0, payload.Length);
                    }
                }
            }    
            catch (Exception Ex)    
            {    
                Console.WriteLine(Ex.ToString());    
            }
            return Package.GetCreated(""+messageId);
        }

        public string UpdateMessage()
        {
            throw new NotImplementedException();
        }
        public string DeleteMessage()
        {
            throw new NotImplementedException();
        }
    }
}
