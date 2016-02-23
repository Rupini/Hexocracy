using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [GameService(GameServiceType.Container)]
    public class GameMap : Map
    {
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

                hexes[centerHex.EntityID] = centerHex;

                for (int i = 0; i < radius; i++)
                {
                    var newHexesAround = new List<Hex>();
                   
                    foreach(var hexAround in hexesAround)
                    {
                        foreach (var hex in hexAround.Neighbors)
                        {
                            if (!hexes.ContainsKey(hex.EntityID))
                            {
                                hexes[hex.EntityID] = hex;
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
