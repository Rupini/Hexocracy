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

        private void r_post_ctor()
        {
            GS.Get<FigureContainer>().OnContentInitialized += OnActorAdded;
            GS.Get<FigureContainer>().OnAdd += OnActorAdded;
            GS.Get<FigureContainer>().OnRemove += OnActorRemoved;
        }

        public List<IActor> GetActorsByPlayer(int playerIndex)
        {
            if (actorsByOwners.ContainsKey(playerIndex))
            {
                return actorsByOwners[playerIndex].Values.ToList();
            }

            return new List<IActor>();
        }

        private void OnActorAdded(IEnumerable<Figure> figures)
        {
            var actors = figures.Cast<IActor>();

            SumActorsByOwners(actors, true);

            Add(figures.Cast<IActor>());
        }

        private void OnActorRemoved(IEnumerable<Figure> figures)
        {
            var actors = figures.Cast<IActor>();

            SumActorsByOwners(actors, false);

            Remove(actors);
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            GS.Get<FigureContainer>().OnContentInitialized -= OnActorAdded;
            GS.Get<FigureContainer>().OnAdd -= OnActorAdded;
            GS.Get<FigureContainer>().OnRemove -= OnActorRemoved;
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
