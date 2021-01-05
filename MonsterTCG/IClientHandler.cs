using System.Net.Sockets;

namespace MonsterTCG
{
    public interface IClientHandler
    {
        public ITcpHandler TcpHandler { get; set; }
        public IContextManager Context { get; set; }
        public RequestManager RequestManager { get; set; }
        public IResponseManager ResponseManager { get; set; }
        public TcpClient Client { get; set; }
        void ExecuteRequest();
        void CloseClient();
    }
}