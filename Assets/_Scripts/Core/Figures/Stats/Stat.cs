using Hexocracy.Mech;
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
        protected virtual event Action<float> ValueChanged = delegate { };

        public StatType Type { get; protected set; }

        public float Value { get; protected set; }

        public Stat(StatType type, float baseValue)
        {
            Type = type;
            _baseValue = baseValue;
        }

        public void SetDependence(List<Stat> components, Func<List<float>, float> dependence)
        {
            if (components != null)
            {
                this.components = components;
                this.components.ForEach(c => c.ValueChanged += OnValueChanged);
            }

            if (dependence == null)
                this.dependence = x => x[0];
            else
                this.dependence = dependence;

            Calculate();
        }

        protected virtual void Calculate()
        {
            var args = new List<float>() { BaseValue };
            components.ForEach(c => args.Add(c.Value));
            Value = dependence(args);
            ValueChanged(Value);
        }

        protected virtual void OnValueChanged(float changedValue)
        {
            Calculate();
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
                    _baseValue = value;
                    OnValueChanged(value);
                }
            }
        }
    }
}
