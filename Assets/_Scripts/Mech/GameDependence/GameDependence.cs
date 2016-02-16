using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class GameDependence<T> : AbstractDependence<T>
    {
        public GameDependence(List<T> arguments)
        {
            this.arguments = arguments;
        }

        public float Calculate(params float[] contextArguments)
        {
            return 0;
        }

    }
}
