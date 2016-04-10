using Hexocracy.CustomEditor;
using System;
using UnityEngine;

namespace Hexocracy
{
    [Serializable]
    public class FigureData
    {
        public int owner;
        public int actionPoints;
        public int baseHP;
        public int damage;
        public int satiety;
        public int baseMass;

        public int jumpingAngle;
        public int jumpUpHeight;
        public int jumpDownHeight;

        [ReadOnly]
        public Color color;
    }
}
