using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace REST_HTTP_Webservice
{
    public class TcpHandler:ITcpHandler
    {
        public TcpHandler(int port, int queue)
        {
            Server = new TcpListener(IPAddress.Loopback, port);
            Server.Start(queue);
        }
        public TcpHandler(int port)
        {
            Server = new TcpListener(IPAddress.Loopback, port);
            Server.Start(5);
        }
        public TcpHandler()
        {
            Server = new TcpListener(IPAddress.Loopback, 8000);
            Server.Start(5);
        }

        public TcpClient Client { get; set; }

        public TcpListener Server { get; set; }

        public void AcceptTcpClient()
        {
            Client = Server.AcceptTcpClient();
        }

        public void CloseClient()
        {
            Client.Close();
        }

        public int DataAvailable()
        {
            return Client.Available;
        }

        public void Dispose()
        {
            Client.Dispose();
        }

        public Stream GetStream()
        {
            return Client.GetStream();
        }

        public void Stop()
        {
            Server.Stop();
        }
    }
}