using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public class StatDependence_GD
    {
        private static void Initialze()
        {
            data = new List<StatDependence_GD>();
            InitialzeInstance(StatType.Red, x => x[0]);
            InitialzeInstance(StatType.Green, x => x[0]);
            InitialzeInstance(StatType.Blue, x => x[0]);

            InitialzeInstance(StatType.Mass, x => x[0] + x[1] +x[2] +x[3], StatType.Red, StatType.Green, StatType.Blue);

            InitialzeInstance(StatType.HealthPoints, x => x[0] + x[1] * 20, StatType.Red);
            InitialzeInstance(StatType.ActionPoints, x => x[0] + x[1], StatType.Blue);

            InitialzeInstance(StatType.MaxDamage, x => x[0] + x[1] * 5, StatType.Green);
            InitialzeInstance(StatType.MinDamage, x => x[1] - x[1] * (0.5f - x[2] / (10 * x[3] + 100)), StatType.MaxDamage, StatType.Mass, StatType.Green);

            InitialzeInstance(StatType.Initiative, x => x[0]);
        }

        private static void InitialzeInstance(StatType Type, Func<List<float>, float> CalculationFunction, params StatType[] links)
        {
            var dataItem = new StatDependence_GD();
            dataItem.Type = Type;
            dataItem.CalculationFunction = CalculationFunction;
            dataItem.Links = links;

            data.Add(dataItem);
        }

        private static List<StatDependence_GD> data;
        public static List<StatDependence_GD> Data
        {
            get
            {
                if(data == null)
                {
                    Initialze();
                }
                return data;
            }
        }


        public StatType Type { get; private set; }

        public StatType[] Links { get; private set; }

        public Func<List<float>, float> CalculationFunction { get; private set; }
    }
}
