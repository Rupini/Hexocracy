using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public partial class StatDependenceData
    {
        #region Static

        private static List<StatDependenceData> statData;
        private static List<StatDependenceData> damageData;
        private static List<StatDependenceData> initializingData;

        public static List<StatDependenceData> Get(DependenceType type)
        {
            if(type == DependenceType.Stat)
            {
                if (statData == null)
                {
                    statData = new List<StatDependenceData>();
                    initializingData = statData;
                    InitialzeStatData();
                }
                return statData;
            }
            else
            {
                if (damageData == null)
                {
                    damageData = new List<StatDependenceData>();
                    initializingData = damageData;
                    InitialzeDamageData();
                }
                return damageData;
            }
        }

        private static void InitialzeInstance(StatType key, Func<List<float>, float> CalculationFunction, params object[] links)
        {
            var dataItem = new StatDependenceData();
            dataItem.statTypeKey = key;
            dataItem.CalculationFunction = CalculationFunction;
            dataItem.Links = links;

            initializingData.Add(dataItem);
        }

        private static void InitialzeInstance(string key, Func<List<float>, float> CalculationFunction, params object[] links)
        {
            var dataItem = new StatDependenceData();
            dataItem.stringKey = key;
            dataItem.CalculationFunction = CalculationFunction;
            dataItem.Links = links;

            initializingData.Add(dataItem);
        }

        
        #endregion
        #region Instance

        private StatType statTypeKey;

        private string stringKey;

        public string Key
        {
            get
            {
                if (stringKey == null)
                    return statTypeKey.ToString();
                else
                    return stringKey;
            }
        }

        public object[] Links { get; private set; }

        public Func<List<float>, float> CalculationFunction { get; private set; }
        #endregion
    }
}
