using Hexocracy.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Hexocracy.Systems
{
    public class GameController : MonoBehaviour
    {
        public static Camera MainCamera { get; private set; }
        public static Canvas Canvas { get; private set; }

        [RawPrototype]
        public List<PlayerData> players;

        private void Awake()
        {
            GS.Initialize();
        }

        private void Start()
        {
            InitHudComponents();

            InitPlayers();

            GS.Get<GameMap>().InitializeContent();
            GS.Get<FigureContainer>().InitializeContent();
            GS.Get<SomeEntityContainer>().InitializeContent();

            GS.Get<ActorContainer>().OnRemove += OnActorsDestroyed;

            GS.Get<TurnController>().Start();
        }

        private void InitPlayers()
        {
            Player.Initialize(players);
        }

        private void InitHudComponents()
        {
            Canvas = Instantiate(Resources.Load<Canvas>("Prefabs/Play/Canvas"));

            MainCamera = FindObjectOfType<Camera>();
            if (!MainCamera)
            {
                MainCamera = Instantiate(Resources.Load<Camera>("Prefabs/Play/MainCamera"));
            }
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
        private void THE_END(Player winner)
        {
            Debug.Log("THE END! " + winner.Name + " is WINNER");
        }
    }
}
