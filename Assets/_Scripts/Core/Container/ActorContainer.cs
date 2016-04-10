using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [GameService(GameServiceType.Container)]
    public class ActorContainer : EntityContainer<IActor>
    {
        private Dictionary<int, Dictionary<int, IActor>> actorsByOwners;

        private ActorContainer()
        {
            actorsByOwners = new Dictionary<int, Dictionary<int, IActor>>();
        }

        public override void InitializeContent()
        {
            var editorActors = GameObject.FindObjectsOfType<ActorEditor>().ToList();

            editorActors.ForEach(editorActor =>
            {
                var actor = editorActor.ToGameInstance();

                entities[actor.EntityID] = actor;
            });

            base.InitializeContent();
        }

        public override void Add(IEnumerable<IActor> entityCollection)
        {
            SumActorsByOwners(entityCollection, true);

            base.Add(entityCollection);
        }

        public override void Remove(IEnumerable<IActor> entityCollection)
        {
            SumActorsByOwners(entityCollection, false);

            base.Remove(entityCollection);
        }

        public List<IActor> GetActorsByPlayer(int playerIndex)
        {
            if (actorsByOwners.ContainsKey(playerIndex))
            {
                return actorsByOwners[playerIndex].Values.ToList();
            }

            return new List<IActor>();
        }

        private void SumActorsByOwners(IEnumerable<IActor> actors, bool added)
        {
            foreach (var actor in actors)
            {
                var playerExist = actorsByOwners.ContainsKey(actor.Owner);

                if (!playerExist && added)
                {
                    actorsByOwners[actor.Owner] = new Dictionary<int, IActor>();
                    playerExist = true;
                }

                if (added && !actorsByOwners[actor.Owner].ContainsKey(actor.EntityID))
                {
                    actorsByOwners[actor.Owner][actor.EntityID] = actor;
                }
                else if (playerExist)
                {
                    actorsByOwners[actor.Owner].Remove(actor.EntityID);
                }

            }
        }
    }
}
