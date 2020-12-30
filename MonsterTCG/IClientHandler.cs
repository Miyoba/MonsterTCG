using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public interface IClientHandler
    {
        void ExecuteRequest();
        void CloseClient();
    }
}