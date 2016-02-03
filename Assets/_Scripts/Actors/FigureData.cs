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
        public float initiative;

        public int jumpingAngle;
        public int jumpUpHeight;
        public int jumpDownHeight;

        public Color color;
    }
}
