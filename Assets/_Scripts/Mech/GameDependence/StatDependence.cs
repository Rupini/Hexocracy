using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
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
                foreach (var item in StatDependenceData.Data)
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

}
