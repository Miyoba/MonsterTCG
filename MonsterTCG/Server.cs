using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterTCG
{
    class Server
    {
        //Amount of clients that can connect at the same time to the server
        static readonly SemaphoreSlim ConcurrentConnections = new SemaphoreSlim(4);

        public static void Main()
        {

            //Preparing the Threads
            TcpHandler tcpHandler = null;
            var tasks = new List<Task>();

            try
            {
                //Using Port 10001 and allowing the default 5 clients in the queue
                tcpHandler = new TcpHandler(10001);

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
            clientHandler.ExecuteRequest();
            clientHandler.CloseClient();
            ConcurrentConnections.Release();
        }
    }
}