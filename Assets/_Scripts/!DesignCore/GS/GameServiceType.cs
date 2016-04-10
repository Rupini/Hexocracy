using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    [Flags]
    public enum GameServiceType : byte
    {
        Dummy = 255,
        Container = 1,
        Factory = 2,
        Model = 4,
        Contoller = 8,
    }
}
