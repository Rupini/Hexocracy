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

        protected Item(ItemBox box)
        {
            this.box = box;
        }

        protected abstract bool OnContact(Figure figure);

        public void Contact(Figure figure)
        {
            if (OnContact(figure)) box.Destroy();
        }
    }
}
