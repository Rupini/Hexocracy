using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public enum StatType : byte
    {
        RedElement = 1,
        GreenElement = 2,
        BlueElement = 3,

        HealthPoints = 10,
        MovePoints = 11,
        ActionPoints = 12,
        Damage = 13
    }
}
