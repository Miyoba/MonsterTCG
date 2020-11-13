using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace REST_HTTP_Webservice
{
    class Server
    {
        static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 8000);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            while (true)
            {
                try
                {
                    var socket = await listener.AcceptTcpClientAsync();
                    using var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
                    writer.WriteLine("Welcome to myserver!");
                    writer.WriteLine("Please enter your commands...");

                    using var reader = new StreamReader(socket.GetStream());
                    string message;
                    var messageList = new List<string>();
                    do
                    {
                        message = reader.ReadLine();
                        Console.WriteLine("Received: " + message);
                        messageList.Add(message);
                    } while (message != "");
                    
                    var package = new HttpPackage(messageList);
                    Console.WriteLine("\n###########################################\n");
                    

                    if (package.ContentLength != 0)
                    {
                        string msg = "";

                        for(int i = 0; i < package.ContentLength; i++)
                        {
                            msg += (char) reader.Read();
                        }
                        package.Payload = msg;
                    }

                    var manager = new MessageManager(package);
                    manager.SaveMessage();


                    Console.WriteLine(package.GetInfo().ToString());
                    string dictionaryString = "{";  
                    foreach(KeyValuePair < string, string > keyValues in package.GetInfo()) {  
                        dictionaryString += keyValues.Key + " : " + keyValues.Value + ", ";  
                    }  
                    Console.WriteLine(dictionaryString.TrimEnd(',', ' ') + "}");
                    socket.Close();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
        }
    }
}