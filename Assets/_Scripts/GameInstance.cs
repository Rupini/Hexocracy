using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class GameInstance : MonoBehaviour
    {
        private FigureFactory figureFactory;
        private List<Player> players;

        private void Awake()
        {
            //Player.Builder.Build(new string[] { "First guy", "Second guy", "Vasia" },
            //    new int[][] { new int[] { 3, 4 }, new int[] { 2, 4 }, new int[] { 2, 3 } },
            //null);

            players = Player.Builder.Build(new string[] { "First guy", "Second guy" },
                                           new int[][] { new int[] { 1 }, new int[] { 0 } },
                                           null);

            var container = new FigureContainer();
            figureFactory = new FigureFactory(container);
            TurnController.Initialize(container);
        }

        private void Start()
        {
            FindObjectsOfType<FigureEditor>().ToList().ForEach(f => f.InitializeForGame());
            TurnController.Start();
        }
    }
}
