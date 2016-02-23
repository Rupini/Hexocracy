using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class FigureDependence : LinkDependece<StatType, Stat>
    {
        public FigureDependence(object mainObject, Dictionary<StatType, Stat> container, Func<List<float>, float> calculationFuncion, params object[] links)
            : base(mainObject, container, calculationFuncion, links)
        {
            converter = (stat) => stat.Value;
        }
        public FigureDependence(Dictionary<StatType, Stat> container, Func<List<float>, float> calculationFuncion, params object[] links)
            : this(null, container, calculationFuncion, links)
        {
        }
    }

}
