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
        public ElementKind kind;
        public int count;

        [ReadOnly]
        public Color color;
    }
}
