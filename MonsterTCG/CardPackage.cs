using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTCG
{
    public class CardPackage:ICardPackage
    {
        public List<ICard> Content
        {
            get => default;
            set
            {
            }
        }

        public int Size
        {
            get => default;
            set
            {
            }
        }

        public int Cost
        {
            get => default;
            set
            {
            }
        }

        public void openPackage(Stack stack)
        {
            throw new NotImplementedException();
        }
    }
}