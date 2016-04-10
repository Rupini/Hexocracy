using Hexocracy.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hexocracy.Systems
{
    [GameService(GameServiceType.Contoller)]
    public class TurnController
    {
        #region Defenition
        private ActorContainer container;

        private List<IActor> actors;
        private int currentFigureIndex;
        private IActor pickedActor;

        private int turnNumber;
        private int currRound;

        private bool GG;

        private bool alreadyStarted;
        #endregion

        #region Initialize
        private TurnController() { }

        private void r_post_ctor()
        {
            container = GS.Get<ActorContainer>();
        }
        #endregion

        #region API

        public event Action<bool> TurnStarted = delegate { };
        public event Action<bool> TurnFinished = delegate { };

        public void Start()
        {
            if(!alreadyStarted)
            {
                alreadyStarted = true;
                Next(true);
            }
            else
            {
                throw new Exception("Game already started!");
            }
        }

        public void OnPlayerFinishedTurn()
        {
            var roundFinished = currentFigureIndex == actors.Count;
            TurnFinished(roundFinished);
            EndTurn(roundFinished);
        }

        #endregion

        #region IMP

        private void Next(bool newRound)
        {
            TurnStarted(newRound);

            if (newRound)
            {
                NextRound();
            }

            if (!GG)
            {
                NextTurn();
            }
            else
            {
                Debug.Log("GG!");
            }
        }

        [RawPrototype]
        private void NextRound()
        {
            actors = container.GetAll();

            if (actors.Count > 0)
            {
                //actors.Sort((f1, f2) => (int)f2.Initiative - (int)f1.Initiative);
                currentFigureIndex = -1;
            }
            else
            {
                GG = true;
            }
        }

        private void NextTurn()
        {
            pickedActor = null;

            while (pickedActor == null && currentFigureIndex + 1 < actors.Count)
            {
                currentFigureIndex++;

                if (!actors[currentFigureIndex].Destroyed)
                {
                    pickedActor = actors[currentFigureIndex];
                }
            }

            if (pickedActor == null)
            {
                TurnFinished(true);
                Next(true);
            }
            else
            {
                pickedActor.Activate();
            }
        }

        private void EndTurn(bool roundFinished)
        {
            pickedActor.Deactivate();
            Next(roundFinished);
        }

        #endregion
    }
}
