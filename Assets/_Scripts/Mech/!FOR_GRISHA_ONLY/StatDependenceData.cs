using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public partial class StatDependenceData
    {
        private static void Initialze()
        {
            data = new List<StatDependenceData>();
            InitialzeInstance(StatType.Red, x => x[0]);
            InitialzeInstance(StatType.Green, x => x[0]);
            InitialzeInstance(StatType.Blue, x => x[0]);

            InitialzeInstance(StatType.Mass, x => x[0] + x[1] + x[2] + x[3], StatType.Red, StatType.Green, StatType.Blue);

            InitialzeInstance(StatType.HealthPoints, x => x[0] + x[1] * 20, StatType.Red);
            InitialzeInstance(StatType.ActionPoints, x => x[0] + x[1], StatType.Blue);

            InitialzeInstance(StatType.MaxDamage, x => x[0] + x[1] * 5, StatType.Green);
            InitialzeInstance(StatType.MinDamage, x => x[1] - x[1] * (0.5f - x[2] / (10 * x[3] + 100)), StatType.MaxDamage, StatType.Mass, StatType.Green);

            InitialzeInstance(StatType.Initiative, x => x[0]);
        }
    }
}
