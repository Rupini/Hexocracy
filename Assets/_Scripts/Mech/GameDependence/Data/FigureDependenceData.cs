using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public partial class FigureDependenceData
    {
        #region Static

        private static List<FigureDependenceData> statData;
        private static List<FigureDependenceData> damageData;

        private static List<FigureDependenceData> initializingData;



        public static List<FigureDependenceData> Get(DependenceType type)
        {
            if (type == DependenceType.Stat)
            {
                if (statData == null)
                {
                    statData = new List<FigureDependenceData>();
                    initializingData = statData;
                    InitialzeStatDependence();
                }
                return statData;
            }
            else
            {
                if (damageData == null)
                {
                    damageData = new List<FigureDependenceData>();
                    initializingData = damageData;
                    InitialzeFigureDependence();
                }
                return damageData;
            }
        }

        private static void InitialzeDependence(Enum elementId, Func<List<float>, float> calculationFunction, params object[] links)
        {
            InitialzeDependence(elementId.ToString(), calculationFunction, links);
        }

        private static void InitialzeDependence(string elementId, Func<List<float>, float> calculationFunction, params object[] links)
        {
            var dataItem = new FigureDependenceData()
            {
                Id = elementId,
                CalculationFunction = calculationFunction,
                Links = links
            };

            initializingData.Add(dataItem);
        }


        #endregion

        #region Instance

        public string Id { get; private set; }

        public object[] Links { get; private set; }

        public float BaseValue { get; private set; }

        public Func<List<float>, float> CalculationFunction { get; private set; }
        #endregion
    }
}
