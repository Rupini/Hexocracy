using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public class StatModifier
    {
        public StatType Target { get; private set; }

        public float Value { get; private set; }

        public bool Factor { get; private set; }
    }
}
