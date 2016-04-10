using Hexocracy.Controller;
using Hexocracy.Controllers;
using Hexocracy.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Hexocracy.Systems
{
    public class GameController : MonoBehaviour
    {
        private void Awake()
        {
            GS.Initialize();
        }

        private void Start()
        {
            Player.Initialize(PlayerPool.GetPlayers());

            GS.Get<GameMap>().InitializeContent();
            GS.Get<FigureContainer>().InitializeContent();
            GS.Get<ActorContainer>().InitializeContent();
            GS.Get<SomeEntityContainer>().InitializeContent();

            GS.Get<ActorContainer>().OnRemove += OnActorsDestroyed;

            GS.Get<TurnController>().Start();

            GS.DestroyTemporary();
        }

        [RawPrototype]
        private void OnActorsDestroyed(IEnumerable<IActor> actors)
        {
            var playerSet = new HashSet<int>();

            foreach (var actor in actors)
            {
                if (!playerSet.Contains(actor.Owner) && GS.Get<ActorContainer>().GetActorsByPlayer(actor.Owner).Count == 0)
                {
                    THE_END(actor.Owner);
                }
                else
                {
                    playerSet.Add(actor.Owner);
                }
            }
        }

        [RawPrototype]
        private void THE_END(Player looser)
        {
            GS.Get<HUDController>().CreateDummyMSG(40, 2.5f, 5, 10, looser.Name + " Is Looser!", Color.red);
            
            GS.Get<TurnController>().Stop();
        }
    }
}
