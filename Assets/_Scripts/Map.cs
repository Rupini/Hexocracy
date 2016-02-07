using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class Map
    {
        protected Dictionary<int, Hex> hexes = new Dictionary<int, Hex>();

        public Map() { }

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
            hexes.Add(hex.Index.GetHashCode(), hex);
        }

        public bool Exist(Index2D index)
        {
            return hexes.ContainsKey(index.GetHashCode());
        }

        public Hex Get(Index2D index)
        {
            if (Exist(index))
                return hexes[index.GetHashCode()];
            else
                return null;
        }
    }
}
