using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IContainable : IEntity
    {
        ContentType Type { get; }

        float Height { get; }

        Player Owner { get; }
    }
}
