using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [GameService(GameServiceType.Factory)]
    public class FigureFactory
    {
        private FigureContainer container;

        private FigureFactory() { }
        
        private void r_post_ctor()
        {
            container = GS.Get<FigureContainer>();
        }

        public void Create(GameObject baseObject, FigureData data)
        {
            var figure = baseObject.AddComponent<Figure>();
            
            figure.Initialize(data);
            
            container.Add(figure);
        }

    }
}
