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
        
        Mass = 5,

        HealthPoints = 10,
        ActionPoints = 11,
        MinDamage = 12,
        MaxDamage = 13,

        Satiety = 20,
        Hunger = 21
    }
}
