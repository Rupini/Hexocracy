using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public static class HexInfo
    {
        private const string PREFAB_PATH = "Prefabs/Play/Hex";
        private const float X_METRIC_K = 1.5f;

        private static Hex _prefab;
        public static Hex Prefab
        {
            get
            {
                if (!_prefab)
                {
                    _prefab = Resources.Load<Hex>(PREFAB_PATH);
                }

                return _prefab;
            }
        }

        private static float? _a;
        public static float A
        {
            get
            {
                if (_a == null)
                {
                    _a = R * 2 * Mathf.Sqrt(1 / 3.0f);
                }

                return _a.Value;
            }
        }

        private static float? _r;
        public static float R
        {
            get
            {
                if (_r == null)
                {
                    _r = Prefab.GetComponent<MeshRenderer>().bounds.size.z * 0.5f;
                }

                return _r.Value;
            }
        }

        private static Vector3? _scale;
        public static Vector3 Scale
        {
            get
            {
                if (_scale == null)
                {
                    _scale = Prefab.transform.localScale;
                }

                return _scale.Value;
            }
        }

        private static float? _h;
        public static float H
        {
            get
            {
                if (_h == null)
                {
                    _h = Prefab.GetComponent<MeshRenderer>().bounds.size.y;
                }

                return _h.Value;
            }
        }

        public static Index2D[] GetCircumIndices(Index2D centerIndex)
        {
            var circumIndices = new Index2D[] 
            { 
                centerIndex.Offset(0,2),
                centerIndex.Offset(1,1),
                centerIndex.Offset(1,-1),
                centerIndex.Offset(0,-2),
                centerIndex.Offset(-1,-1),
                centerIndex.Offset(-1,1)
            };

            return circumIndices;
        }

        public static Vector3 IndexToVector(Index2D index, float height = 0)
        {
            return new Vector3(index.X * X_METRIC_K * A, height, index.Y * R);
        }

        public static Index2D VectorToIndex(Vector3 vector)
        {
            return new Index2D((int)(vector.x / (X_METRIC_K * A)), (int)(vector.z / R));
        }
    }
}
