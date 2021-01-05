using System.IO;
using System.Net.Sockets;

namespace MonsterTCG
{
    public interface ITcpHandler
    {
        TcpClient AcceptTcpClient();
        int DataAvailable(TcpClient client);
        Stream GetStream(TcpClient client);
        void Stop();
        void CloseClient(TcpClient client);
    }
}