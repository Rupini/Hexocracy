using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public class DynamicStat : Stat
    {
        private float _currValue;
        public float CurrValue
        {
            get
            {
                return _currValue;
            }
            set
            {
                if (value > Value) value = Value;

                if (_currValue != value)
                {
                    CurrentValueChaged(value);
                    _currValue = value;
                }
            }
        }

        public DynamicStat(StatType type, float baseValue)
            : this(type, baseValue, baseValue)
        {
        }

        public DynamicStat(StatType type, float baseValue, float currValue)
            : base(type, baseValue)
        {
            _currValue = currValue;
        }

        public event Action<float> CurrentValueChaged = delegate { };

        protected override void OnValueChanged(float changedValue)
        {
            float lastValue = Value;
            base.OnValueChanged(changedValue);
            CurrValue *= Value / lastValue;
        }
    }
}
