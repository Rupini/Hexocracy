using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class GlobalItemSpawner : TurnListenerBehaviour
    {
        private List<GlobalItemSpawnerAgent> agents;

        public void Initialize(GameMap map, GlobalItemSpawnerData data)
        {
            agents = new List<GlobalItemSpawnerAgent>();
            foreach(var agentData in data.agents)
            {
                agents.Add(new GlobalItemSpawnerAgent(map, agentData));
            }
        }

        protected override void OnTurnStarted(bool newRound)
        {
            if(newRound)
            {
                agents.ForEach(agent => agent.Spawning());
            }
        }
    }
}
