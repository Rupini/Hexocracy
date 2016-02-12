using Hexocracy.CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    [Serializable]
    public class ItemData
    {
        public const int OFFSET_DEFAULT_FLAG = -1000;

        public ItemType type;
        public ElementKind kind;
        public int count;

        [HideInInspector]
        public int lifeTime;

        [HideInInspector]
        public float yOffsetK = OFFSET_DEFAULT_FLAG;

        [ReadOnly]
        public Color color;
    }
}
