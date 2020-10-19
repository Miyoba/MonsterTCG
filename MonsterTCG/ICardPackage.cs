using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public interface ICardPackage
    {
        int Size { get; set; }
        List<ICard> Content { get; set; }
        int Cost { get; set; }

        void openPackage(Stack stack);
    }
}