using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class LinkDependece<TLink, T> : AbstractDependence<T>
    {

        public abstract class AbstractProvider
        {
            protected Dictionary<TLink, LinkDependece<TLink, T>> dependencies;
            protected Dictionary<TLink, T> container;

            public AbstractProvider(Dictionary<TLink, T> container)
            {
                dependencies = new Dictionary<TLink, LinkDependece<TLink, T>>();
                this.container = container;

                InitializeDependencies();
            }

            protected abstract void InitializeDependencies();

            public LinkDependece<TLink, T> Get(TLink link)
            {
                return dependencies[link];
            }
        }

        protected LinkDependece(Dictionary<TLink, T> container, Func<List<float>, float> calculationFuncion, params TLink[] links)
        {
            CalculationFunction = calculationFuncion;
            arguments = new List<T>();
            foreach (var link in links)
            {
                arguments.Add(container[link]);
            }
        }
    }
}
