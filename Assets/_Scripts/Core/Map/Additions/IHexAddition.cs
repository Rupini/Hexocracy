using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    partial interface IHexAddition
    {
        void Attach(Hex target);
        void Disattach();

        void OnTurnUpdate(bool isNewRound);

    }
}
