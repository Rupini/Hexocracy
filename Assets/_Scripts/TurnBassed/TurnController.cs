using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class TurnController
    {
        private const int ROUNDS_IN_CYCLE = 5;

        public static event Action<bool> RoundStarted = delegate { };
        public static event Action RoundFinished = delegate { };
        private static TurnController instance;

        private int turnNumber;
        private List<PlayerController> controllers;
        private PlayerController currController;
        private int currRound;

        public static void Start(List<PlayerController> controllers)
        {
            instance = new TurnController(controllers);
            instance.ActivateNextPlayer();
        }

        public static void OnPlayerFinishedTurn()
        {
            instance.FinishTurnForPlayer();
        }

        private TurnController(List<PlayerController> pControllers)
        {
            controllers = new List<PlayerController>(pControllers);
            
            controllers = pControllers;
            controllers.Sort((p1, p2) => { return p1.TurnOrder - p2.TurnOrder; });
            turnNumber = 0;
            currRound = 0;
        }

        private void ActivateNextPlayer()
        {
            if (turnNumber == 0)
                StartRound(currRound == 0);

            currController = controllers[turnNumber];
            currController.Activate();
            turnNumber++;
        }

        private void FinishTurnForPlayer()
        {
            currController.Deactivate();
            if (turnNumber >= controllers.Count)
            {
                FinishRound();
                turnNumber = 0;
                currRound++;
                if (currRound >= ROUNDS_IN_CYCLE)
                    currRound = 0;
            }

            ActivateNextPlayer();
        }


        private void StartRound(bool isNewCicle)
        {
            RoundStarted(isNewCicle);
        }

        private void FinishRound()
        {
            RoundFinished();
        }
    }
}
