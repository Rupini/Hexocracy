using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public static class Mechanics
    {
        public static int CalculateMovementCost(Hex currHex, Hex nextHex)
        {
            return nextHex.HasFigure && nextHex.H > currHex.H ? nextHex.H - currHex.H + 1 : 1;
        }
    }
}
