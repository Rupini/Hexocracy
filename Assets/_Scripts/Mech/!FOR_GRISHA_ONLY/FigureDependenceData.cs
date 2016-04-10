using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Mech
{
    public partial class FigureDependenceData
    {
        private static void InitialzeStatDependence()
        {
            InitialzeDependence(StatType.Red, x => x[0]);
            InitialzeDependence(StatType.Green, x => x[0]);
            InitialzeDependence(StatType.Blue, x => x[0]);

            InitialzeDependence(StatType.Mass, x => x[0] + x[1] + x[2] + x[3], StatType.Red, StatType.Green, StatType.Blue);

            InitialzeDependence(StatType.HealthPoints, x => x[0] + x[1] * 20, StatType.Red);
            InitialzeDependence(StatType.ActionPoints, x => x[0] + Mathf.FloorToInt((0.2f + 0.2f * (1 + x[0]) / (2 + x[1])) * x[1]), StatType.Blue);

            InitialzeDependence(StatType.MaxDamage, x => x[0] + x[1] * 5, StatType.Green);
            InitialzeDependence(StatType.MinDamage, x => x[1] - x[1] * (0.5f - x[2] / (10 * x[3] + 100)), StatType.MaxDamage, StatType.Mass, StatType.Green);

            InitialzeDependence(StatType.Hunger, x => x[1], StatType.Mass);

            InitialzeDependence(StatType.Satiety, x => x[0]);
        }

        private static void InitialzeFigureDependence()
        {
            InitialzeDependence(FigureDependenceType.SimpleAttack, x => x[0] + x[1] * x[2], "Damage", StatType.Mass);
            InitialzeDependence(FigureDependenceType.PenaltiAllyAttack, x => (x[0] + x[3] * x[1]) * x[2] * 0.5f, "Damage", StatType.Mass, "AP");
            InitialzeDependence(FigureDependenceType.PenanltiFalling, x => x[0] * x[1], StatType.Mass);
            InitialzeDependence(FigureDependenceType.PenaltiForcedMove, x => x[0] * (-x[1]), "Damage", "AP");
            InitialzeDependence(FigureDependenceType.APRegenByTurn, x => x[0], "MaxAP");
            InitialzeDependence(FigureDependenceType.DeptionByTurn, x => x[0] - x[1], StatType.Satiety, StatType.Hunger);
            InitialzeDependence(FigureDependenceType.HPLoseByHunger, x => x[0] + (x[1] >= 0 ? 0 : x[1]), "HP", StatType.Satiety);
            InitialzeDependence(FigureDependenceType.SaturationByElement, x => x[0] + 15 * x[1], StatType.Satiety);
        }
    }
}
