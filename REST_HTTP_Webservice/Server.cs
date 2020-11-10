using System;
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
                    string messageComplete = "";
                    do
                    {
                        message = reader.ReadLine();
                        Console.WriteLine("Received: " + message);
                        messageComplete += message;
                    } while (message != "");
                    
                    var temp = new HttpPackage(messageComplete);
                    Console.WriteLine("\n###########################################\n");
                    

                    if (temp.ContentLength != 0)
                    {
                        string msg = "";

                        for(int i = 0; i < temp.ContentLength; i++)
                        {
                            msg += (char) reader.Read();
                        }
                        temp.Payload = msg;
                    }

                    Console.WriteLine(temp.GetInfo());
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