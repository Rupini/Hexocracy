using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    [Serializable]
    public class BombData : ItemData
    {
        public float damage;

        public override ItemType type { get { return ItemType.Other; } }
    }
}
