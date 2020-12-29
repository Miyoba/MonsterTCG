using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace REST_HTTP_Webservice
{
    class ClientHandler:IClientHandler
    {
        public ClientHandler(ITcpHandler tcpHandler)
        {
            TcpHandler = tcpHandler;
            TcpHandler.AcceptTcpClient();
        }

        public ITcpHandler TcpHandler { get; set; }
        public IRequestContext Request { get; set; }
        public MessageManager RequestManager { get; set; }
        public IResponse ExecuteRequest()
        {
            StreamReader reader = new StreamReader(TcpHandler.GetStream());
            Request = new RequestContext(reader);
            if (Request.CheckRequest())
            {

                RequestManager = new MessageManager(Request);
                var text = RequestManager.ProcessRequest();
                TextResponse response = new TextResponse(StatusCodesEnum.Ok, text);
                return response;
            }
            return new TextResponse(StatusCodesEnum.BadRequest, Request.GetBadRequest("Bad Request!"));
        }

        public void SendResponse(IResponse response)
        {
            using StreamWriter writer = new StreamWriter(TcpHandler.GetStream()) { AutoFlush = true };
            writer.WriteLine(response.Content);
        }

        public void CloseClient()
        {
            TcpHandler.CloseClient();
        }
    }
}
