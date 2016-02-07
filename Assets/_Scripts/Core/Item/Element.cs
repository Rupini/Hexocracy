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

        public Element(ItemBox box, ItemData data)
            : base(box)
        {
            Kind = data.kind;
            Count = data.count;
        }

        public override void Apply(Figure figure)
        {
            base.Apply(figure);
            figure.AddElement(this);
        }
    }
}
