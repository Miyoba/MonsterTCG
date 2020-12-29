using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REST_HTTP_Webservice
{
    public interface IClientHandler
    {
        IResponse ExecuteRequest();
        void SendResponse(IResponse response);
        void CloseClient();
    }
}