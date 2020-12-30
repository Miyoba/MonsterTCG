using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
    {
    public class TextResponse:IResponse
    {
        public TextResponse(StatusCodesEnum status, string content)
        {
            Status = status;
            ContentType = "text/plain";
            Content = content;
        }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public StatusCodesEnum Status { get; set; }
    }
}