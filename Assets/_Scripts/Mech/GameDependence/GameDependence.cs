using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public abstract class GameDependence<T>
    {
        protected List<T> arguments;

        public Func<List<float>, float> CalculationFunction { get; protected set; }

        public List<T> GetArguments()
        {
            return arguments;
        }
    }
}
