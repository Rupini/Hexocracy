using System;
using System.Collections.Generic;
using URandom = UnityEngine.Random;

namespace Hexocracy.HelpTools
{
    public class Range
    {
        public static implicit operator float(Range range)
        {
            return range.GetValue();
        }

        private Func<float> minValue;
        private Func<float> maxValue;

        public Range(Func<float> minValue, Func<float> maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public float GetValue()
        {
            return URandom.Range(minValue(), maxValue());
        }

        public override string ToString()
        {
            return (int)minValue() + "-" + (int)maxValue();
        }
    }
}
