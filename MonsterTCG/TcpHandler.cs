using System.IO;
using System.Net;
using System.Net.Sockets;

namespace MonsterTCG
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

        public TcpListener Server { get; set; }

        public TcpClient AcceptTcpClient()
        {
            return Server.AcceptTcpClient();
        }

        public void CloseClient(TcpClient client)
        {
            client.Close();
        }

        public int DataAvailable(TcpClient client)
        {
            return client.Available;
        }

        public Stream GetStream(TcpClient client)
        {
            return client.GetStream();
        }

        public void Stop()
        {
            Server.Stop();
        }
    }
}