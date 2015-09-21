using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class Stat
    {
        #region Static
        public static implicit operator float(Stat stat)
        {
            return stat.Value;
        }

        public static implicit operator int(Stat stat)
        {
            return (int)Mathf.Round(stat.Value);
        }
        #endregion
        protected float _baseValue;
        protected List<Stat> components = new List<Stat>();
        protected Func<List<float>, float> dependence = delegate { return 0; };

        public StatType Type { get; protected set; }

        public float Value { get; protected set; }

        public virtual event Action<float> ValueChanged = delegate { };


        public Stat(StatType type, float baseValue, List<Stat> components = null, Func<List<float>, float> dependence = null)
        {
            Type = type;
            _baseValue = baseValue;
            if (components != null)
            {
                this.components = components;
                this.components.ForEach((c) => { c.ValueChanged += OnValueChanged; });
            }
            if (dependence != null) this.dependence = dependence;

            ValueChanged += OnValueChanged;

            Calculate();
        }

        protected virtual void Calculate()
        {
            Value = BaseValue;
            Value += dependence(components.ConvertAll(c => c.Value));
        }

        protected virtual void OnValueChanged(float changedValue)
        {
            Calculate();
            ValueChanged(Value);
        }

        public float BaseValue
        {
            get
            {
                return _baseValue;
            }
            set
            {
                if (_baseValue != value)
                {
                    ValueChanged(value);
                    _baseValue = value;
                }
            }
        }
    }
}
