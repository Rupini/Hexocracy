using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class TurnController
    {
        #region Static
        public static event Action<bool> TurnStarted = delegate { };
        public static event Action TurnFinished = delegate { };
        private static TurnController instance;

        public static void Initialize(FigureContainer container)
        {
            instance = new TurnController(container);
        }

        public static void Start()
        {
            instance.Next(true);
        }

        public static void OnPlayerFinishedTurn()
        {
            TurnFinished();
            instance.EndTurn();
        }
        #endregion

        private FigureContainer container;

        private List<Figure> figures;
        private int currentFigureIndex;
        private Figure pickedFigure;

        private int turnNumber;
        private int currRound;

        private bool GG = false;

        private TurnController(FigureContainer container)
        {
            this.container = container;
        }

        private void Next(bool newRound)
        {
            TurnStarted(newRound);

            if (newRound) NextRound();

            if (!GG)
                NextTurn();
            else
                Debug.Log("GG!");
        }

        private void NextRound()
        {
            figures = container.GetFigures();
            if (figures.Count > 0)
            {
                figures.Sort((f1, f2) => (int)f2.Initiative - (int)f1.Initiative);
                currentFigureIndex = -1;
            }
            else
                GG = true;
        }

        private void NextTurn()
        {
            pickedFigure = null;

            while (pickedFigure == null && currentFigureIndex + 1 < figures.Count)
            {
                currentFigureIndex++;
                pickedFigure = figures[currentFigureIndex];
            }

            if (pickedFigure == null)
            {
                TurnFinished();
                Next(true);
            }
            else
                pickedFigure.Activate();
        }

        private void EndTurn()
        {
            pickedFigure.Deactivate();
            Next(currentFigureIndex == figures.Count);
        }
    }
}
