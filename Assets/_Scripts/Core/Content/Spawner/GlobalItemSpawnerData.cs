using Hexocracy.CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    [Serializable]
    public class GlobalItemSpawnerData
    {
        public Agent[] agents;

        [Serializable]
        public class Agent
        {
            public HexEditor originHex;

            public float minCountPerTurn;
            public float maxCountPerTurn;

            public int limitCount;
            public int limitDistanceFromCenter;

            public ItemType itemType;

            [Condition(true, "itemType", "Element")]
            public ElementData element;

            public ItemData Item
            {
                get
                {
                    return element;
                }
            }

        }

    }
}
