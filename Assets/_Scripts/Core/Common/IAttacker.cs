using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IAttacker
    {
        float Damage { get; }
        Player Owner { get; }
    }
}
