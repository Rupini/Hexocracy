using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class Building : MapObject
    {
        public override ContentType Type { get { return ContentType.Building; } }

        public abstract BuildingType BType { get; }
    }
}
