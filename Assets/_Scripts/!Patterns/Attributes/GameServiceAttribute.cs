using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public class GameServiceAttribute : Attribute
    {
        public GameServiceType ServiceType { get; private set; }

        public GameServiceAttribute(GameServiceType serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
