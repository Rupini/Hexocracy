using Hexocracy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.CustomEditor
{
    public class MapGenerator
    {
        protected HexEditor prototype;
        protected Transform container;
        protected float a;
        protected float r;

        private int circleCount;
        private int currCC;
        private Dictionary<int, HexEditor> hexMap;

        public MapGenerator(HexEditor prototype, int circleCount)
        {
            this.prototype = prototype;
            this.container = new GameObject("Hex Container").transform;
            this.circleCount = circleCount;
            r = prototype.GetComponent<Renderer>().bounds.size.z * 0.5f;
            a = r * 2 * Mathf.Sqrt(1 / 3.0f);
            hexMap = new Dictionary<int, HexEditor>();
        }

        public void Generate()
        {
            var firstHex = CreateHex(new Index2D(0, 0));
            currCC = 1;
            BuildCircle(new List<HexEditor> { firstHex });
        }

        public Transform GetContainer()
        {
            return container;
        }

        private void BuildCircle(List<HexEditor> hexes)
        {
            List<HexEditor> builtHexes = new List<HexEditor>();

            hexes.ForEach((h) => { builtHexes.AddRange(BuildAround(h)); });

            currCC++;

            if (currCC < circleCount)
                BuildCircle(builtHexes);
        }

        private List<HexEditor> BuildAround(HexEditor hex)
        {
            List<HexEditor> hexes = new List<HexEditor>();

            foreach (var index in hex.NeighborsIndexes)
                if (!hexMap.ContainsKey(index.GetHashCode()))
                {
                    var builtHex = CreateHex(index);
                    hexes.Add(builtHex);
                }

            return hexes;
        }

        private HexEditor CreateHex(Index2D index)
        {
            var hex = ((HexEditor)GameObject.Instantiate(prototype, new Vector3(1.5f * a * index.X, 0, r * index.Y), new Quaternion()));
            hexMap.Add(index.GetHashCode(), hex);
            hex.Initialize(index, container);
            return hex;
        }
    }
}
