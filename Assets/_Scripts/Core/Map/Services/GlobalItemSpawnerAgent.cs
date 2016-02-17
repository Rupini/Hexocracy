using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Hexocracy.HelpTools;
using URandom = UnityEngine.Random;

namespace Hexocracy.Core
{
    public class GlobalItemSpawnerAgent
    {
        private GameMap map;

        private ItemData itemPrototype;

        private float minCountPerTurn;
        private float maxCountPerTurn;

        private bool unlimitCount;
        private int limitCount;
        private int currentCountOnMap;

        private float awaitingCount;
        private float lastAwaitingCount;

        private bool unlimitDistanceFromCenter;
        private int limitDistanceFromCenter;

        public GlobalItemSpawnerAgent(GameMap map, GlobalItemSpawnerData.Agent data)
        {
            this.map = map;

            minCountPerTurn = data.minCountPerTurn;
            maxCountPerTurn = data.maxCountPerTurn;

            limitCount = data.limitCount;
            unlimitCount = limitCount == 0;

            limitDistanceFromCenter = data.limitDistanceFromCenter;
            unlimitDistanceFromCenter = limitDistanceFromCenter == 0;

            itemPrototype = data.Item;
        }

        public void Spawning()
        {
            lastAwaitingCount = awaitingCount;
            awaitingCount += Mathf.Round(URandom.Range(minCountPerTurn, maxCountPerTurn) * 10) / 10;

            float permittedAwaitingCount;

            if (unlimitCount)
                permittedAwaitingCount = awaitingCount;
            else
                permittedAwaitingCount = Mathf.Min(awaitingCount, limitCount - currentCountOnMap);

            if (permittedAwaitingCount >= 1)
            {
                var preparedCount = Mathf.FloorToInt(awaitingCount);

                if (preparedCount > 0)
                {
                    awaitingCount = awaitingCount - preparedCount;

                    var availableHexes = map.GetAll().FindAll(FindCondition);

                    if (availableHexes.Count > 0)
                    {
                        var spawnedCount = Spawn(preparedCount, availableHexes);
                        currentCountOnMap += spawnedCount;

                        //if (awaitingCount > spawnedCount)
                        //{
                        //    awaitingCount = lastAwaitingCount;
                        //}
                    }
                    else
                    {
                        awaitingCount = lastAwaitingCount;
                    }
                }
            }
            else
            {
                if (awaitingCount > permittedAwaitingCount)
                {
                    awaitingCount = lastAwaitingCount;
                }
            }
        }


        private int Spawn(int itemCount, List<Hex> hexes)
        {
            int spawnedCount = 0;
            for (int i = 0; i < itemCount; i++)
            {
                int hexIndex;
                var hex = hexes.GetRandom(out hexIndex);
                if (hex != null)
                {
                    var itemBox = ItemFactory.I.Create(itemPrototype, hex);
                    itemBox.OnDestroy += OnItemDestroy;
                    hexes.RemoveAt(hexIndex);
                    spawnedCount++;
                }
            }
            return spawnedCount;
        }

        private bool FindCondition(Hex hex)
        {
            return hex.Content.Type == ContentType.Empty &&
                (unlimitDistanceFromCenter || (Mathf.Abs(hex.Index.X) + Mathf.Abs(hex.Index.Y * 0.5f)) <= limitDistanceFromCenter);
        }

        private void OnItemDestroy()
        {
            currentCountOnMap--;
        }



    }
}
