using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    [RawPrototype]
    public class FigureContainer
    {
        private Dictionary<int, Figure> figures;

        public FigureContainer()
        {
            figures = new Dictionary<int, Figure>();
        }

        public void OnFigureCreated(Figure figure)
        {
            figures[figure.GetInstanceID()] = figure;
        }

        public void OnFigureRemoved(Figure figure)
        {
            figures.Remove(figure.GetInstanceID());
        }

        public List<Figure> GetFigures()
        {
            return figures.Values.ToList();
        }
    }
}
