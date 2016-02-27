using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Hexocracy.Core
{
    public class ItemSpawner : HexAddition
    {
        private int count;
        private ItemType itemType;
        private ElementKind kind;
        private int minRespTime;
        private int maxRespTime;

        private bool firstSpawn;

        private int roundCounter;
        private int requiredRound;

        public ItemSpawner(HexData.Addition data)
        {
            count = data.count;
            kind = data.kind;
            itemType = data.type;
            minRespTime = data.minRespawnTime;
            maxRespTime = data.maxRespawnTime;
            firstSpawn = data.respawnOnStart;

            roundCounter = 0;
            requiredRound = URandom.Range(minRespTime, maxRespTime + 1);
        }

        private void OnTurnStarted(bool newRound)
        {
            if (newRound)
            {
                if (firstSpawn || roundCounter == requiredRound)
                {
                    firstSpawn = false;
                    roundCounter = 0;
                    requiredRound = URandom.Range(minRespTime, maxRespTime + 1);
                    Spawn();
                }
                else
                {
                    roundCounter++;
                }
            }
        }

        private void Spawn()
        {
            if (owner.Content.Type == ContentType.Empty)
            {
                var data = new ElementData();
                data.capacity = count;
                data.kind = kind;

                ItemFactory.I.Create(data, owner);
            }
        }

        protected override void OnAttach(Hex target)
        {
            TurnController.TurnStarted += OnTurnStarted;
        }

        protected override void OnDisattach()
        {
            TurnController.TurnStarted -= OnTurnStarted;
        }
    }
}
