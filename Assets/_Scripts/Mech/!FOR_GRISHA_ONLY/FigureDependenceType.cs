using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public enum FigureDependenceType
    {
        SimpleAttack = 1,
        PenaltiAllyAttack = 2,
        PenanltiFalling = 3,
        PenaltiForcedMove = 4,
        APRegenByTurn= 5,
        DeptionByTurn = 6,
        HPLoseByHunger = 7,
        SaturationByElement = 8
    }
}
