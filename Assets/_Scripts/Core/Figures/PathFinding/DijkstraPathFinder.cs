﻿using Hexocracy.Mech;
using System;
using System.Collections.Generic;
using Hexocracy.HelpTools;
using UnityEngine;

namespace Hexocracy.Core
{
    public class DijkstraPathFinder : PathFinder
    {
        private const int UNDEFINED = int.MaxValue;
        private const int OBSTACLE = int.MaxValue / 2;

        private Dictionary<int, Hex> H; // Dirty vertices
        private Dictionary<int, int> D; // Distance from origin to the hex
        private Dictionary<int, PathVertex> P; // Previus for current
        private RelaxList<Item, Hex> Q; // Control priority changed

        private class Item : IRelaxable<Hex>
        {
            public Hex Value { get; set; }
            public int Cost { get; set; }
        }

        public DijkstraPathFinder(int jumpUpHeight, int jumpDownHeight)
            : base(jumpUpHeight, jumpDownHeight)
        {
            H = new Dictionary<int, Hex>();
            D = new Dictionary<int, int>();
            P = new Dictionary<int, PathVertex>();
            Q = new RelaxList<Item, Hex>();
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
            InsertItem(origin, 0);
            P[origin] = null;
        }

        private bool Find()
        {
            for (int i = 0; i < GameMap.I.Count - 1; i++)
            {
                Hex v = GetItemWithMinWeight();
                H[v] = v;

                foreach (var u in v.Neighbors)
                {
                    if (!H.ContainsKey(u))
                    {
                        if (!D.ContainsKey(u))
                        {
                            D[u] = UNDEFINED;
                            InsertItem(u, UNDEFINED);
                        }

                        var cost = GetCost(v, u);

                        if (D[v] + cost < D[u])
                        {
                            D[u] = D[v] + cost;
                            ChageItemWeight(u, D[u]);
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
                    //Broken path condition
                    if ((vertex.Cost) >= OBSTACLE) return new Path();
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
            destination = Q.FindAll((item) => { return isTarget(item.Value); }).GetRandom().Value;
            return BuildPathToDestination();
        }


        private int GetCost(Hex v, Hex u)
        {
            if (CheckHeight(v, u, forced) && checkPassibility(u) == PassibilityType.Ok)
                return Mechanics.CalculateMovementCost(v, u);
            else
                return OBSTACLE;
        }

        private void InsertItem(Hex u, int d)
        {
            Q.Push(new Item() { Value = u, Cost = d });
        }

        private void ChageItemWeight(Hex u, int d)
        {
            Q.Relax(u, d);
        }

        private Hex GetItemWithMinWeight()
        {
            return Q.Pop().Value;
        }
    }
}