using System.IO;

namespace MonsterTCG
{
    class ClientHandler:IClientHandler
    {
        public ITcpHandler TcpHandler { get; set; }
        public IContextManager Context { get; set; }
        public RequestManager RequestManager { get; set; }
        public IResponseManager ResponseManager { get; set; }

        public ClientHandler(ITcpHandler tcpHandler)
        {
            TcpHandler = tcpHandler;
            TcpHandler.AcceptTcpClient();
        }

        public void ExecuteRequest()
        {
            StreamReader reader = new StreamReader(TcpHandler.GetStream());
            Context = new ContextManager(reader);
            RequestManager = new RequestManager(Context);
            SendResponse(RequestManager.ProcessRequest());
        }

        public void SendResponse(IResponse response)
        {
            ResponseManager = new ResponseManager(response, "MonsterTCG-Server");
            using StreamWriter writer = new StreamWriter(TcpHandler.GetStream()) { AutoFlush = true };
            writer.WriteLine(ResponseManager.ProcessResponse());
        }

        public void CloseClient()
        {
            TcpHandler.CloseClient();
        }
    }
}
