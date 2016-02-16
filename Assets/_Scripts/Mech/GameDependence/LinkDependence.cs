using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    #region GameDesign

    public class StatDependence : LinkDependece<StatType, Stat>
    {
        public class Provider : AbstractProvider
        {
            public Provider(Dictionary<StatType, Stat> container)
                : base(container)
            {

            }

            protected override void InitializeDependencies()
            {
                foreach (var item in StatDependence_GD.Data)
                {
                    var dependence = new StatDependence(container, item.CalculationFunction, item.Links);
                    dependencies[item.Type] = dependence;
                }
            }
        }

        protected StatDependence(Dictionary<StatType, Stat> container, Func<List<float>, float> calculationFuncion, params StatType[] links)
            : base(container, calculationFuncion, links)
        {
        }
    }

    #endregion
    #region Logic
    public class LinkDependece<TLink, T> : GameDependence<T>
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
    #endregion
}
