using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class FigureFactory
    {
        public static FigureFactory I { get; private set; }

        private FigureContainer container;

        public FigureFactory(FigureContainer container)
        {
            this.container = container;
            I = this;
        }

        public void Create(GameObject baseObject, FigureData data)
        {
            var figure = baseObject.AddComponent<Figure>();
            figure.Initialize(container, data);
            GameObject.Destroy(baseObject.GetComponent<FigureEditor>());
        }

    }
}
