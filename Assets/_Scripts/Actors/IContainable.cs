using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public interface IContainable
    {
        float Height { get; }

        ContentType Type { get; }

        Player Owner { get; }

        bool Destroyed { get; }

        void Destroy();
    }
}
