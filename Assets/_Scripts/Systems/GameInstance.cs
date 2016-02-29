using Hexocracy.HelpTools;
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
            GameServices.Get<GameMap>().Initialize();

            GameObjectUtility.FindObjectsOfInterfaceType<IEditorBehaviour>().ForEach(editor => editor.InitGameInstance());

            //var sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            //GameServices.Get<Nihility>().OnProcessComplete += OnProcessComplete;
            //GameServices.Get<Nihility>().ToProcess();
            //sw.Stop();
            //Debug.Log(sw.ElapsedMilliseconds);

            //TurnController.TurnFinished += OnRoundEnd;

            TurnController.Start();
        }

        private List<GameObject> balls = new List<GameObject>();

        private void OnProcessComplete()
        {
            balls.RemoveAll(ball => { Destroy(ball); return true; });

            var nihility = GameServices.Get<Nihility>();

            for (int i = 0; i < nihility.SectorCount; i++)
            {
                var sector = nihility.GetSector(i);
                foreach (var hex in sector.Values)
                {
                    Debug.Log(i + ") " + hex.Index);

                    var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    var trans = go.transform;
                    trans.position = HexInfo.IndexToVector(hex.Index);
                    trans.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
                    balls.Add(go);
                }
            }
        }

        private void OnRoundEnd(bool roundEnd)
        {
            if (roundEnd)
            {
                var list = new List<Hex>();
                var hex = GameServices.Get<GameMap>().GetAll().GetRandom();

                foreach (var neighboor in hex.Neighbors)
                {
                    list.Add(neighboor);
                    neighboor.Destroy();
                }

                list.Add(hex);
                hex.Destroy();

                GameServices.Get<GameMap>().Remove(list);
            }
        }
    }
}
