using Hexocracy.CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [Serializable]
    public class HexData
    {
        [ReadOnly]
        public int xIndex, yIndex;
        [ReadOnly]
        public float scaleY;
        [Range(1, 10)]
        public int height;

        public bool hasAddition;

        [Condition("hasAddition","true")]
        public Addition addition;

        [Serializable]
        public class Addition
        {
            public ItemType type;

            [Condition("type", "Element")]
            public ElementKind kind;

            public int count;
            public int minRespawnTime;
            public int maxRespawnTime;
            public bool respawnOnStart;
        }
    }
}
