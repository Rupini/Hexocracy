using Hexocracy.CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    [Serializable]
    public abstract class ItemData
    {
        public abstract ItemType type { get; }

        public int capacity;

        public int lifeTime;

        public bool overrideOffsetK;

        [Condition(true, "overrideOffsetK", "true")]
        public float yOffsetK = 0;

        [HideInInspector]
        public Color color;
    }
}
