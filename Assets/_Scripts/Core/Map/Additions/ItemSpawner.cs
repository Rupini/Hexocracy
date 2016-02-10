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
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                var position = owner.GroundCenter;
                position.y += 2;
                go.transform.position = position;

                var itemBox = go.AddComponent<ItemBox>();

                GameObject.Destroy(go.GetComponent<BoxCollider>());

                var data = new ItemData();

                data.count = count;

                if (kind == ElementKind.Random || kind == ElementKind.None)
                    data.kind = (ElementKind)URandom.Range(1, 4);
                else
                    data.kind = kind;

                itemBox.Initialize(data);
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
