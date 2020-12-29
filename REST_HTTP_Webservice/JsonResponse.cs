using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REST_HTTP_Webservice
{
    public class JsonResponse:IResponse
    {
        public JsonResponse(StatusCodesEnum status, string content)
        {
            Status = status;
            ContentType = "application/json";
            Content = content;
        }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public StatusCodesEnum Status { get; set; }
    }
}