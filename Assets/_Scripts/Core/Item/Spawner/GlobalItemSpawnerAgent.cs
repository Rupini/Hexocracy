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

        private Hex originHex;

        private float minCountPerTurn;
        private float maxCountPerTurn;

        private bool unlimitCount;
        private int limitCount;
        private int currentCountOnMap;

        private float awaitingCount;
        private float spawnCancelAwaitingCount;

        private bool unlimitDistanceFromCenter;
        private int limitDistanceFromCenter;

        public GlobalItemSpawnerAgent(GlobalItemSpawnerData.Agent data)
        {
            map = GS.Get<GameMap>();

            originHex = data.originHex.GameInstance;

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
            spawnCancelAwaitingCount = 0;// awaitingCount;
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

                    List<Hex> availableHexes;
                    if (unlimitDistanceFromCenter)
                    {
                        availableHexes = map.GetAll().FindAll(hex => hex.Content.Type == ContentType.Empty);
                    }
                    else
                    {
                        availableHexes = map.GetHexInArea(originHex, limitDistanceFromCenter).FindAll(hex => hex.Content.Type == ContentType.Empty);
                    }

                    if (availableHexes.Count > 0)
                    {
                        var spawnedCount = Spawn(preparedCount, availableHexes);
                        currentCountOnMap += spawnedCount;

                        if (awaitingCount > spawnedCount)
                        {
                            awaitingCount = spawnCancelAwaitingCount;
                        }
                    }
                    else
                    {
                        awaitingCount = spawnCancelAwaitingCount;
                    }
                }
            }
            else
            {
                if (awaitingCount > permittedAwaitingCount)
                {
                    awaitingCount = spawnCancelAwaitingCount;
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
                    var itemBox = GS.Get<ItemFactory>().Create(itemPrototype, hex);
                    itemBox.OnDestroy += OnItemDestroy;
                    hexes.RemoveAt(hexIndex);
                    spawnedCount++;
                }
            }
            return spawnedCount;
        }

        private void OnItemDestroy()
        {
            currentCountOnMap--;
        }



    }
}
