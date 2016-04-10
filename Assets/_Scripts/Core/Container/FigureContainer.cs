using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [GameService(GameServiceType.Container)]
    public class FigureContainer : EntityContainer<Figure>
    {
        private FigureContainer() { }

        public override void InitializeContent()
        {
            var editorFigures = GameObject.FindObjectsOfType<FigureEditor>().ToList();

            editorFigures.ForEach(editorFigure =>
            {
                var figure = editorFigure.ToGameInstance();

                entities[figure.EntityID] = figure;
            });

            base.InitializeContent();
        }
       
    }
}
