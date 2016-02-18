using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class GameMap : Map
    {
        private static GameMap _i;
        public static GameMap I
        {
            get
            {
                if (_i == null) _i = new GameMap();
                return _i;
            }
        }

        public List<Hex> GetAll()
        {
            return hexes.Values.ToList();
        }

        public List<Hex> GetHexInArea(int xIndex, int yIndex, int radius)
        {
            var hexes = new Dictionary<int, Hex>();
            var centerHex = Get(new Index2D(xIndex, yIndex));
            
            if (centerHex != null)
            {
                var hexesAround = new List<Hex>();
                hexesAround.Add(centerHex);

                hexes[centerHex.GetHashCode()] = centerHex;

                for (int i = 0; i < radius; i++)
                {
                    var newHexesAround = new List<Hex>();
                   
                    foreach(var hexAround in hexesAround)
                    {
                        foreach (var hex in hexAround.Neighbors)
                        {
                            if (!hexes.ContainsKey(hex.GetHashCode()))
                            {
                                hexes[hex.GetHashCode()] = hex;
                                newHexesAround.Add(hex);
                            }
                        }
                    }

                    hexesAround = newHexesAround;
                }
            }

            return hexes.Values.ToList();
        }

        public void ResetFlags(int undefinedValue)
        {
            foreach (var pair in hexes)
            {
                pair.Value.FindFlag = undefinedValue;
                pair.Value.ChangeColor(Color.white);
            }
        }
    }
}
