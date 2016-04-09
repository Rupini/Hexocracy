using Hexocracy.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Hexocracy.Systems
{
    public class GameController : MonoBehaviour
    {
        public static Camera MainCamera { get; private set; }
        public static Canvas Canvas { get; private set; }

        private void Awake()
        {
            GS.Initialize();

            Player.Builder.Build(new string[] { "First guy", "Second guy" },
                                         new int[][] { new int[] { 1 }, new int[] { 0 } },
                                         null);

            InitializeHudComponents();
        }

        private void InitializeHudComponents()
        {
            Canvas = Instantiate(Resources.Load<Canvas>("Prefabs/Play/Canvas"));
            MainCamera = Instantiate(Resources.Load<Camera>("Prefabs/Play/MainCamera"));
        }

        private void Start()
        {
            GS.Get<GameMap>().InitializeContent();
            GS.Get<FigureContainer>().InitializeContent();
            GS.Get<SomeEntityContainer>().InitializeContent();

            GS.Get<TurnController>().Start();
        }

    }
}
