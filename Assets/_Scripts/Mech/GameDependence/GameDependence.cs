using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class GameDependence<T> : AbstractDependence<T>
    {
        protected Converter<T, float> converter;

        public GameDependence(Func<List<float>, float> calculationFunction, List<T> arguments, Converter<T, float> converter)
        {
            CalculationFunction = calculationFunction;
            this.arguments = arguments;
        }

        public float Calculate(params float[] contextArguments)
        {
            var currArgs = arguments.ConvertAll(arg=>converter(arg));
            currArgs.AddRange(contextArguments);
            return CalculationFunction(currArgs);
        }

    }
}
