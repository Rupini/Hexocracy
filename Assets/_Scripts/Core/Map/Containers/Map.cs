using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class Map
    {
        protected Dictionary<int, Hex> hexes;

        public Map()
        {
            hexes = new Dictionary<int, Hex>();
        }

        public Index2D MinIndex { get; protected set; }

        public Index2D MaxIndex { get; protected set; }

        public int Count
        {
            get
            {
                return hexes.Count;
            }
        }

        public void Clear()
        {
            hexes.Clear();
        }

        public void Add(Hex hex)
        {
            hexes.Add(hex.EntityID, hex);
            DefineMainIndices(hex.Index);
        }

        public bool Exist(Index2D index)
        {
            return hexes.ContainsKey(index);
        }

        public Hex Get(Index2D index)
        {
            if (Exist(index))
                return hexes[index];
            else
                return null;
        }

        public bool InMapRange(Index2D index)
        {
            return index.X <= MaxIndex.X && index.Y <= MaxIndex.Y && index.X >= MinIndex.X && index.Y >= MinIndex.Y;
        }

        private void DefineMainIndices(Index2D index)
        {
            if (index.X > MaxIndex.X) MaxIndex = new Index2D(index.X, MaxIndex.Y);
            if (index.Y > MaxIndex.Y) MaxIndex = new Index2D(MaxIndex.X, index.Y);
            if (index.X < MinIndex.X) MinIndex = new Index2D(index.X, MinIndex.Y);
            if (index.Y < MinIndex.Y) MinIndex = new Index2D(MinIndex.X, index.Y);
        }
    }
}
