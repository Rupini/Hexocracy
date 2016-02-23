using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class FigureDependenceProvider : LinkDependenceProvider<StatType, Stat>
    {
        private DependenceType dependenceType;

        public FigureDependenceProvider(object mainObject, Dictionary<StatType, Stat> container, DependenceType dependenceType)
            : base(mainObject, container)
        {
            this.dependenceType = dependenceType;
            InitializeDependencies();
        }

        public FigureDependenceProvider(Dictionary<StatType, Stat> container, DependenceType dependenceType)
            : this(null, container, dependenceType)
        {

        }

        protected void InitializeDependencies()
        {
            foreach (var item in FigureDependenceData.Get(dependenceType))
            {
                var dependence = new FigureDependence(mainObject, container, item.CalculationFunction, item.Links);
                dependencies[item.Id.ToString()] = dependence;
            }
        }

    }
}
