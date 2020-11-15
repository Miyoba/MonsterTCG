using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
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
                    using var writer = new StreamWriter(socket.GetStream()/*,Encoding.UTF8*/) {AutoFlush = true};
                    using var reader = new StreamReader(socket.GetStream()/*,Encoding.UTF8*/);
                    var package = new RequestContext(reader);
                    Console.WriteLine("\n###########################################\n");

                    Console.WriteLine("Analyzed Information:\r\n");
                    foreach(KeyValuePair<string, string> entry in package.Information)
                    {
                        Console.WriteLine(entry.Key+": "+entry.Value);
                    }
                    Console.WriteLine("\n###########################################\n");
                    if (package.CheckRequest()){ 
                        var manager = new MessageManager(package);
                        await writer.WriteAsync(manager.ProcessRequest());
                    }
                    else
                        writer.Write(package.GetBadRequest("Bad request!"));

                    if(socket.Connected)
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