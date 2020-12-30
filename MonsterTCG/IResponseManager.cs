using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public interface IResponseManager
    {
        IResponse Response { get; set; }
        string ProcessResponse();
    }
}