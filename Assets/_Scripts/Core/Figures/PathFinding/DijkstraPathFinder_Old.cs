using Hexocracy.Mech;
using System;
using System.Collections.Generic;
using Hexocracy.HelpTools;
using UnityEngine;

namespace Hexocracy.Core
{
    public class DijkstraPathFinder_Old : PathFinder
    {
        private const int UNDEFINED = int.MaxValue;
        private const int OBSTACLE = int.MaxValue / 2;

        private Dictionary<int, Hex> H; // Dirty vertices
        private Dictionary<int, int> D; // Distance from origin to the hex
        private Dictionary<int, PathVertex> P; // Previus for current

        private Dictionary<int, Item> Q; // Binary Heap with Hex & Priority

        private class Item
        {
            public Hex hex { get; set; }
            public int priority { get; set; }
        }

        public DijkstraPathFinder_Old(int jumpUpHeight, int jumpDownHeight)
            : base(jumpUpHeight, jumpDownHeight)
        {
            H = new Dictionary<int, Hex>();
            D = new Dictionary<int, int>();
            P = new Dictionary<int, PathVertex>();

            Q = new Dictionary<int, Item>();
        }

        protected override Path FindOriginalPath(Hex origin, Hex destination = null, bool forced = false, Func<Hex, PassibilityType> checkPassibiliy = null, Func<Hex, bool> isTarget = null)
        {
            Initialize(origin);

            if (Find())
                return BuildPath();
            else
                return new Path();
        }

        private void Initialize(Hex origin)
        {
            H.Clear();
            D.Clear();
            P.Clear();
            Q.Clear();

            D[origin] = 0;
            PriorityInsert(origin, 0);
            P[origin] = null;
        }

        private bool Find()
        {
            for (int i = 0; i < map.Count - 1; i++)
            {
                Hex v = PriorityGetMin();
                H[v] = v;

                foreach (var u in v.Neighbors)
                {
                    if (!H.ContainsKey(u))
                    {
                        if (!D.ContainsKey(u))
                        {
                            D[u] = UNDEFINED;
                            PriorityInsert(u, UNDEFINED);
                        }

                        var cost = GetCost(v, u);

                        if (D[v] + cost < D[u])
                        {
                            D[u] = D[v] + cost;
                            PriorityDecrease(u, D[u]);
                            P[u] = new PathVertex(v, cost);
                        }
                    }
                }
            }

            return destination == null || P.ContainsKey(destination);
        }

        private Path BuildPath()
        {
            if (destination != null)
                return BuildPathToDestination();
            else
                return BuildPathWithCondition();
        }


        private Path BuildPathToDestination()
        {
            List<PathVertex> vertices = new List<PathVertex>();

            if (destination != null)
            {
                var vertex = new PathVertex(destination, 0);

                while (vertex != null)
                {
                    vertices.Add(vertex);
                    vertex = P[vertex.Hex];
                }

                vertices.Reverse();

                return new Path(vertices);
            }
            else
                return new Path();
        }

        private Path BuildPathWithCondition()
        {
            var items = new List<Item>(Q.Values);
            items.Sort((q1, q2) => { return q1.priority - q2.priority; });
            destination = items.Find((q) => { return isTarget(q.hex); }).hex;

            return BuildPathToDestination();
        }


        private int GetCost(Hex v, Hex u)
        {
            if (CheckHeight(v, u, forced) && checkPassibility(u) == PassibilityType.Ok)
                return Mechanics.CalculateMovementCost(v, u);
            else
                return OBSTACLE;
        }

        private void PriorityInsert(Hex u, int d)
        {
            Q[u] = new Item() { hex = u, priority = d };
        }

        private void PriorityDecrease(Hex u, int d)
        {
            Q[u] = new Item() { hex = u, priority = d };
        }

        private Hex PriorityGetMin()
        {
            int min = UNDEFINED;
            Hex hex = null;
            foreach (var pair in Q)
            {
                if (!H.ContainsKey(pair.Value.hex) && pair.Value.priority < min)
                {
                    min = pair.Value.priority;
                    hex = pair.Value.hex;
                }
            }
            return hex;
        }
    }
}
