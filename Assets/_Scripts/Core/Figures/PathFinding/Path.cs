using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class Path
    {
        private List<PathVertex> vertices;
        private int counter = 0;

        public bool Useful { get; private set; }
        public int Count { get { return vertices.Count - 1; } }
        public int TotalCost { get; private set; }
        public Hex CurrentHex { get { return Useful ? vertices[counter].Hex : null; } }
        public int CurrentCost { get { return Useful ? vertices[counter - 1].Cost : int.MaxValue; } }

        public Path()
        {
            Useful = false;
        }

        public Path(List<Hex> hexes, int cost)
        {
            this.vertices = hexes.ConvertAll((h) => new PathVertex(h, cost));
            Useful = vertices != null && vertices.Count > 0;
            TotalCost = cost * vertices.Count;
        }

        public Path(List<PathVertex> vertices)
        {
            this.vertices = vertices;
            Useful = vertices != null && vertices.Count > 0;
            TotalCost = vertices.Sum(v => v.Cost);
        }

        public bool Next()
        {
            if (!Useful) return false;

            counter++;
            if (counter < vertices.Count)
                return true;
            else
            {
                Useful = false;
                return false;
            }
        }

        public void Break()
        {
            Useful = false;
            vertices.Clear();
        }

       
    }
}
