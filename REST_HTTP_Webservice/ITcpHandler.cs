using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace REST_HTTP_Webservice
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