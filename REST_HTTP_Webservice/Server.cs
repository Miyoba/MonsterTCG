using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace REST_HTTP_Webservice
{
    class Server
    {
        //Amount of clients that can connect at the same time to the server
        static readonly SemaphoreSlim ConcurrentConnections = new SemaphoreSlim(2);

        public static void Main()
        {

            //Preparing the Threads
            TcpHandler tcpHandler = null;
            var tasks = new List<Task>();

            try
            {
                //Using standard Port 8000 and allowing 5 clients in the queue
                tcpHandler = new TcpHandler();

                while (true)
                {
                    //Starting the Threads
                    ConcurrentConnections.Wait();
                    tasks.Add(Task.Run(() => ReceiveClient(tcpHandler)));

                }
            }
            //Hopefully no Exceptions here
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            finally
            {
                //When finished stop the Server and set all threads on hold
                tcpHandler?.Stop();
                Task.WaitAll(tasks.ToArray());
            }
        }
        /*
         * The Code that should be realized via Thread.
         * In my case the client connection and Request and Response Handling
         */
        private static void ReceiveClient(ITcpHandler tcpHandler)
        {
            ClientHandler clientHandler = new ClientHandler(tcpHandler);
            IResponse response = clientHandler.ExecuteRequest();
            clientHandler.SendResponse(response);
            clientHandler.CloseClient();
            ConcurrentConnections.Release();
        }
    }
}