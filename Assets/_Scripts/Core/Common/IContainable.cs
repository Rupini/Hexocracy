using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IContainable
    {
        ContentType Type { get; }

        float Height { get; }

        Player Owner { get; }

        bool Destroyed { get; }

        void Destroy();
    }
}
