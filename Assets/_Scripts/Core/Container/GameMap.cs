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
        public override event Action<IEnumerable<Hex>> OnAdd = delegate { };
        public override event Action<IEnumerable<Hex>> OnRemove = delegate { };

        public override void InitializeContent()
        {
            var editorHexes = GameObject.FindObjectsOfType<HexEditor>().ToList();

            var hexes = editorHexes.ConvertAll(editorHex =>
            {
                var hex = editorHex.ToGameInstance();
                DefineMainIndicesOnAdd(hex.Index);
                entities[hex.EntityID] = hex;
                return hex;
            });

            foreach (var hex in hexes)
            {
                hex.DefineCircum();
            }

            base.InitializeContent();
        }

        public void ResetFlags(int undefinedValue)
        {
            foreach (var hex in entities.Values)
            {
                hex.FindFlag = undefinedValue;
            }
        }

        public Index2D MinIndex { get; protected set; }

        public Index2D MaxIndex { get; protected set; }

        public override void Add(IEnumerable<Hex> hexes)
        {
            //foreach (var hex in hexes)
            //{
            //    DefineMainIndicesOnAdd(hex.Index);
            //    entities[hex.EntityID] = hex;
            //}

            foreach (var hex in hexes)
            {
                foreach (var curcumHex in hex.Circum)
                {
                    curcumHex.DefineCircum();
                }
            }

            base.Add(hexes);

            //OnAdd(hexes);
        }

        public override void Remove(IEnumerable<Hex> hexes)
        {
            //foreach (var hex in hexes)
            //{
            //    DefineMainIndicesOnRemove(hex.Index);
            //    entities.Remove(hex.EntityID);
            //}

            foreach (var hex in hexes)
            {
                foreach (var curcumHex in hex.Circum)
                {
                    curcumHex.DefineCircum();
                }
            }

            base.Remove(hexes);

            //OnRemove(hexes);
        }

        public bool InMapRange(Index2D index)
        {
            return index.X <= MaxIndex.X && index.Y <= MaxIndex.Y && index.X >= MinIndex.X && index.Y >= MinIndex.Y;
        }

        public List<Hex> GetHexInArea(Hex originHex, int radius)
        {
            var hexes = new Dictionary<int, Hex>();

            if (originHex != null)
            {
                var hexesAround = new List<Hex>();
                hexesAround.Add(originHex);

                hexes[originHex.EntityID] = originHex;

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

        private void DefineMainIndicesOnAdd(Index2D index)
        {
            if (index.X > MaxIndex.X) MaxIndex = new Index2D(index.X, MaxIndex.Y);
            if (index.Y > MaxIndex.Y) MaxIndex = new Index2D(MaxIndex.X, index.Y);
            if (index.X < MinIndex.X) MinIndex = new Index2D(index.X, MinIndex.Y);
            if (index.Y < MinIndex.Y) MinIndex = new Index2D(MinIndex.X, index.Y);
        }

        private void DefineMainIndicesOnRemove(Index2D index)
        {
            if (index.X == MaxIndex.X) MaxIndex = new Index2D(index.X - 1, MaxIndex.Y);
            if (index.Y == MaxIndex.Y) MaxIndex = new Index2D(MaxIndex.X, index.Y-1);
            if (index.X == MinIndex.X) MinIndex = new Index2D(index.X+1, MinIndex.Y);
            if (index.Y == MinIndex.Y) MinIndex = new Index2D(MinIndex.X, index.Y+1);
        }
    }
}
