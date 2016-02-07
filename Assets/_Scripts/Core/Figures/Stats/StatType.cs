using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public enum StatType : byte
    {
        Red = 1,
        Green = 2,
        Blue = 3,

        HealthPoints = 10,
        ActionPoints = 11,
        Damage = 12,
        Initiative = 13
    }
}
