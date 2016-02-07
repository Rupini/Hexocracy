using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class PathVertex
    {
        public Hex Hex { get; private set; }

        public int Cost { get; private set; }

        public PathVertex(Hex hex, int cost)
        {
            this.Hex = hex;
            this.Cost = cost;
        }
    }
}
