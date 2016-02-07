using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class Item
    {
        protected ItemBox box;

        public abstract ItemType Type { get; }

        public virtual void Apply(Figure figure)
        {
            box.Destroy();
        }
        
        public Item(ItemBox box)
        {
            this.box = box;
        }
    }
}
