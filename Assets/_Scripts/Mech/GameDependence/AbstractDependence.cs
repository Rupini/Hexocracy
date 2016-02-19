using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public abstract class AbstractDependence<T>
    {
        protected List<T> argumentsOfType;
        protected List<object> allArguments;

        public Func<List<float>, float> CalculationFunction { get; protected set; }

        public List<T> GetArgumentsOfType()
        {
            return argumentsOfType;
        }
    }
}
