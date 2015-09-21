using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class GameInstance : MonoBehaviour
    {
        private List<PlayerController> controllers;

        private void Awake()
        {
            //Player.Builder.Build(new string[] { "First guy", "Second guy", "Vasia" },
            //    new int[][] { new int[] { 3, 4 }, new int[] { 2, 4 }, new int[] { 2, 3 } },
            //null);

            var players = Player.Builder.Build(new string[] { "First guy", "Second guy" },
               new int[][] { new int[] { 1 }, new int[] { 0 } },
           null);

            controllers = PlayerController.Builder.Build(players);
        }

        private void Start()
        {
            TurnController.Start(controllers);
        }
    }
}
