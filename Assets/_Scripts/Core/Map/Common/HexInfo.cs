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
        public const float X_METRIC_K = 1.5f;
        
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
    }
}
