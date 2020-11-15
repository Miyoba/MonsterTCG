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
        public IRequestContext Package { get; set; }

        public MessageManager(IRequestContext package)
        {
            Package = package;
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

        public string ProcessRequest()
        {
            switch (Package.Request.ToUpper())
            {
                case "GET":
                    string[] separator = {"/"};
                    string[] words = Package.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length == 1)
                        return ListMessages();
                    return ReadMessage();
                case "POST":
                    return SaveMessage();
                case "PUT":
                    return UpdateMessage();
                case "DELETE":
                    return DeleteMessage();
                default:
                    return Package.GetBadRequest("Bad header request: "+Package.Request);
            }
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

        public string GetMessageDirPath()
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
                return directoryInfo.FullName + Package.Path;
            return null;
        }

        public string ReadMessage()
        {
            string[] separator = {"/"};
            string[] words = Package.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            if(words.Length == 2){
                if (string.Equals(words[0], "messages"))
                {
                    var current = GetMessageDirPath();
                    var text = "";
                    if (File.Exists(current))
                    {
                        text += File.ReadAllText(current);
                        if (string.Equals(text, ""))
                            return Package.GetNoContent("Message is Empty!");
                        return Package.GetOk(text);
                    }
                    return Package.GetNotFound("No File "+Package.Path+" found!");
                }
            }
            return Package.GetBadRequest("Unknown Directory-Path!");
        }

        public string ListMessages()
        {
            string[] separator = {"/"};
            string[] words = Package.Path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            if (string.Equals(words[0], "messages"))
            {
                var list = "";
                var current = GetMessageDirPath();
                foreach (string file in Directory.EnumerateFiles(current))
                {
                    separator[0] = current + "\\";
                    words = file.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                    list += words[0]+"\n";
                }

                if (string.Equals(list, ""))
                    return Package.GetNoContent("No content found.");
                return Package.GetOk(list);
            }

            return Package.GetBadRequest("Unknown path.");
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
                return Package.GetBadRequest("Internal Error!");
            }
            return Package.GetCreated(""+messageId);
        }

        public string UpdateMessage()
        {
            var current = GetMessageDirPath();
            try    
            {    
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(current))    
                {    
                    File.Delete(current);    
                }    
        
                // Create a new file     
                using (FileStream fs = File.Create(current))     
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
                return Package.GetBadRequest("Internal Error!");
            }
            return Package.GetCreated("Message Changed!");
        }
        public string DeleteMessage()
        {
            var current = GetMessageDirPath();
            if (File.Exists(current))    
            {    
                File.Delete(current);
                return Package.GetOk("File " + Package.Path + " successfully deleted!");
            }
            return Package.GetNotFound("File " + Package.Path + " not found!");
        }
    }
}
