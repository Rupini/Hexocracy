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
            // Call from GameMap!
            hexes.ForEach(hex => hex.ToGameInstance());
            hexes.ForEach(hex => hex.GetComponent<Hex>().DefineCircum());

            FindObjectsOfType<EditorBehaviour>().ToList().ForEach(editorObj => editorObj.ToGameInstance());

            //var sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            GameServices.Get<Nihility>().ToProcess();
            //sw.Stop();
            //Debug.Log(sw.ElapsedMilliseconds);

            //var nihility = GameServices.Get<Nihility>();

            //for (int i = 0; i < nihility.SectorCount; i++)
            //{
            //    var sector = nihility.GetSector(i);
            //    foreach (var hex in sector.Values)
            //    {
            //        Debug.Log(i + ") " + hex.Index);

            //        var trans = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            //        trans.position = new Vector3(HexInfo.X_METRIC_K * HexInfo.A * hex.Index.X, 0, HexInfo.R * hex.Index.Y);
            //        trans.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
            //    }

              
            //}

            TurnController.Start();
        }
    }
}
