using System.IO;

namespace MonsterTCG
{
    public interface ITcpHandler
    {
        void AcceptTcpClient();
        int DataAvailable();
        Stream GetStream();
        void Dispose();
        void Stop();
        void CloseClient();
    }
}