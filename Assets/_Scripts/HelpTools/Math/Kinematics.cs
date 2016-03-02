using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.HelpTools
{
    public static class Kinematics
    {
        private static float g;

        static Kinematics()
        {
            g = -Physics.gravity.y;
        }

        public static Vector3 CalculateVelocity(Vector3 p1, Vector3 p2, float angle, out float t)
        {
            Vector3 r = p2 - p1;

            float dy = p2.y - p1.y;

            float dx = Mathf.Sqrt(r.x * r.x + r.z * r.z);

            t = Mathf.Sqrt(2 * (dx * Mathf.Tan(angle) - dy) / g);

            float v0 = dx / (t * Mathf.Cos(angle));

            r = new Vector3(r.x, dx * Mathf.Tan(angle), r.z);

            return r.normalized * v0;
        }

        public static Vector3 CalculateVelocity(float height, out float t)
        {
            float vy = Mathf.Sqrt(2 * height * g);
            t = 2 * vy / g;

            return new Vector3(0, vy, 0);
        }
    }
}
