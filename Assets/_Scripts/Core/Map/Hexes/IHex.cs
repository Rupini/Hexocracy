using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IHex : IEntity
    {
        bool Defined { get; }
        bool IsNihility { get; }
        Index2D Index { get; }
        IHex[] Circum { get; }
        Index2D[] CircumIndices { get; }
    }
}
