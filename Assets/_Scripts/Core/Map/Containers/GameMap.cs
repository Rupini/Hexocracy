using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [GameService(GameServiceType.Container)]
    public class GameMap : EntityContainer<Hex>
    {
        public void Initialize()
        {
            var editorHexes = GameObject.FindObjectsOfType<HexEditor>().ToList();
            var hexes = editorHexes.ConvertAll(editorHex => 
            {
                var hex = editorHex.ToGameInstance();
                entities[hex.EntityID] = hex;
                return hex;
            });

            foreach(var hex in hexes)
            {
                hex.DefineCircum();
            }
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

                    foreach (var hexAround in hexesAround)
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
            foreach (var pair in entities)
            {
                pair.Value.FindFlag = undefinedValue;
                pair.Value.ChangeColor(Color.white);
            }
        }

        public Index2D MinIndex { get; protected set; }

        public Index2D MaxIndex { get; protected set; }

        public override void Add(IEnumerable<Hex> hexes)
        {
            foreach (var hex in hexes)
            {
                DefineMainIndices(hex.Index);
            }
            base.Add(hexes);
        }

        public override void Remove(IEnumerable<Hex> hexes)
        {
            base.Remove(hexes);
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
