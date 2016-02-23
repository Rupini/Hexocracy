using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public partial class FigureDependenceData
    {
        private static void InitialzeStatData()
        {
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

        private static void InitialzeDamageData()
        {
            InitialzeInstance(DamageDepenenceType.SimpleAttack, x => x[0] + x[1] * x[2], "Damage", StatType.Mass);
            InitialzeInstance(DamageDepenenceType.PenaltiAllyAttack, x => (x[0] + x[3] * x[1]) * x[2] * 0.5f, "Damage", StatType.Mass, "AP");
            InitialzeInstance(DamageDepenenceType.PenanltiFalling, x => x[0] * x[1], StatType.Mass);
            InitialzeInstance(DamageDepenenceType.PenaltiForcedMove, x => x[0] * (-x[1]), "Damage", "AP");
        }
    }
}
