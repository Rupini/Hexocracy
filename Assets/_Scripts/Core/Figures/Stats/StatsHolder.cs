using Hexocracy.Mech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class StatsHolder
    {
        private Dictionary<StatType, Stat> stats;

        public StatsHolder()
        {
            stats = new Dictionary<StatType, Stat>();
        }

        public Stat this[StatType type]
        {
            get
            {
                return stats[type];
            }
        }

        public Stat Add(StatType type, float baseValue, bool isDynamic)
        {
            Stat stat = null;

            if (isDynamic)
                stat = new DynamicStat(type, baseValue);
            else
                stat = new Stat(type, baseValue);

            stats.Add(stat.Type, stat);

            return stat;
        }

        public Stat Get(StatType type)
        {
            return stats[type];
        }


        public void InitializeDependencies()
        {
            var provider = new StatDependenceProvider(stats, DependenceType.Stat);

            foreach(var pair in stats)
            {
                var dependence = provider.Get(pair.Key);
                pair.Value.SetDependence(dependence.GetArgumentsOfType(), dependence.CalculationFunction);
            }
        }

        public Dictionary<StatType, Stat> GetStats()
        {
            return stats;
        }
    }
}
