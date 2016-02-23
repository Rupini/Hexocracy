using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class GameInstance : MonoBehaviour
    {
        private FigureFactory figureFactory;
        private List<Player> players;
        private Canvas canvas;
        private Camera mainCamera;

        public static Canvas Canvas { get; private set; }

        private void Awake()
        {
            GameServices.Initialize();

            InitializeMainComponents();

            players = Player.Builder.Build(new string[] { "First guy", "Second guy" },
                                           new int[][] { new int[] { 1 }, new int[] { 0 } },
                                           null);

            var container = new FigureContainer();
            figureFactory = new FigureFactory(container);
            TurnController.Initialize(container);
        }

        private void InitializeMainComponents()
        {
            canvas = Instantiate(Resources.Load<Canvas>("Prefabs/Play/Canvas"));
            mainCamera = Instantiate(Resources.Load<Camera>("Prefabs/Play/MainCamera"));

            Canvas = canvas;
        }

        private void Start()
        {
            var hexes = FindObjectsOfType<HexEditor>().ToList();

            hexes.ForEach(hex => hex.ToGameInstance());
            hexes.ForEach(hex => hex.GetComponent<Hex>().PostInitialize());

            FindObjectsOfType<EditorBehaviour>().ToList().ForEach(editorObj => editorObj.ToGameInstance());

            GameServices.Get<Nihility>().ToProcess();

            foreach (var hex in GameServices.Get<Nihility>().GetAll())
                Debug.Log(hex.Index + " nihility = " + hex.NihilitySectorIndex);

            TurnController.Start();
        }
    }
}
