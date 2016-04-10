using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class GlobalItemSpawner : TurnListenerBehaviour, IEntity
    {
        private List<GlobalItemSpawnerAgent> agents;

        public bool Destroyed { get; private set; }

        public int EntityID { get { return GetInstanceID(); } }

        public event Action<IEntity> OnDestroy = delegate { };

        public void Initialize(GlobalItemSpawnerData data)
        {
            agents = new List<GlobalItemSpawnerAgent>();
            foreach(var agentData in data.agents)
            {
                agents.Add(new GlobalItemSpawnerAgent(agentData));
            }
        }

        protected override void OnTurnStarted(bool newRound)
        {
            if(newRound)
            {
                agents.ForEach(agent => agent.Spawning());
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            Destroyed = true;
            OnDestroy(this);
            Destroy(go);
        }
    }
}
