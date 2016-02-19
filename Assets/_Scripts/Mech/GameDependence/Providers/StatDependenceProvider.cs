using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class StatDependenceProvider : LinkDependenceProvider<StatType, Stat>
    {
        private DependenceType dependenceType;

        public StatDependenceProvider(object mainObject, Dictionary<StatType, Stat> container, DependenceType dependenceType)
            : base(mainObject, container)
        {
            this.dependenceType = dependenceType;
            InitializeDependencies();
        }

        public StatDependenceProvider(Dictionary<StatType, Stat> container, DependenceType dependenceType)
            : this(null, container, dependenceType)
        {

        }

        protected void InitializeDependencies()
        {
            foreach (var item in StatDependenceData.Get(dependenceType))
            {
                var dependence = new StatDependence(mainObject, container, item.CalculationFunction, item.Links);
                dependencies[item.Key.ToString()] = dependence;
            }
        }

    }
}
