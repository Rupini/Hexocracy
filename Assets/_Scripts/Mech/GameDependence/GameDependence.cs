using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class GameDependence<T> : AbstractDependence<T>
    {
        protected Converter<T, float> converter;

        protected GameDependence()
        {

        }

        public GameDependence(Func<List<float>, float> calculationFunction, List<T> arguments, Converter<T, float> converter)
        {
            CalculationFunction = calculationFunction;
            this.argumentsOfType = arguments;
        }

        public float Calculate(params float[] contextArguments)
        {
            var currArgs = allArguments.ConvertAll(arg =>
            {
                if (arg.GetType() == typeof(Func<float>))
                    return InvokeGetter(arg);
                else
                    return converter((T)arg);
            });
            currArgs.AddRange(contextArguments);
            return CalculationFunction(currArgs);
        }

        public float InvokeGetter(object getter)
        {
            return ((Func<float>)getter)();
        }

    }
}
