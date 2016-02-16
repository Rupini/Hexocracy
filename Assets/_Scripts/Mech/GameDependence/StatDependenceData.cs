using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public partial class StatDependenceData
    {
        #region Static
        private static void InitialzeInstance(StatType Type, Func<List<float>, float> CalculationFunction, params StatType[] links)
        {
            var dataItem = new StatDependenceData();
            dataItem.Type = Type;
            dataItem.CalculationFunction = CalculationFunction;
            dataItem.Links = links;

            data.Add(dataItem);
        }

        private static List<StatDependenceData> data;
        public static List<StatDependenceData> Data
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
        #endregion
        #region Instance
        public StatType Type { get; private set; }

        public StatType[] Links { get; private set; }

        public Func<List<float>, float> CalculationFunction { get; private set; }
        #endregion
    }
}
