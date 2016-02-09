using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IActivableObject : IEntity
    {
        bool Active { get; }

        void Activate();

        void Deactivate();
    }
}
