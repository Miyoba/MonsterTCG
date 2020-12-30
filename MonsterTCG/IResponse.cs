using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public interface IResponse
    {
        String Content { get; set; }
        string ContentType { get; set; }
        StatusCodesEnum Status { get; set; }
    }
}