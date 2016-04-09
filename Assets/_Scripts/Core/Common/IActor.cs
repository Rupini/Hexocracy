using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IActor : IEntity
    {
        bool Active { get; }

        void Activate();

        void Deactivate();
    }
}
