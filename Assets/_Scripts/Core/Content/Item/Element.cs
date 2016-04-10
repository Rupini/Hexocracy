using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class Element : Item
    {
        public override ItemType Type { get { return ItemType.Element; } }

        public ElementKind Kind { get; private set; }

        public int Count { get; private set; }

        public Element(ItemBox box, ElementData data)
            : base(box)
        {
            Kind = data.kind;
            Count = data.capacity;
        }

        protected override bool OnContact(Figure figure)
        {
            figure.AddElement(this);
            return true;
        }
    }
}
