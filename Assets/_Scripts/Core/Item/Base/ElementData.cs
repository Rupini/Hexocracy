using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    [Serializable]
    public class ElementData : ItemData
    {
        public ElementKind kind;

        public override ItemType type { get { return ItemType.Element; } }
       
    }
}
