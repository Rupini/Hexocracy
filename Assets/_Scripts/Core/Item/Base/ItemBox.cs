using Hexocracy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class ItemBox : MapObject
    {
        public override ContentType Type { get { return ContentType.Item; } }

        public Item Item { get; private set; }

        private ItemBox()
        {
            Height = 0;
            Owner = Player.NeutralPassive;
        }

        public void Initialize(ItemData data)
        {
            Item = new Element(this, data);

            r.material = Resources.Load<Material>("Models/Materials/whiteColor");
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteColor");
            r.material.color = data.color;

            DefineStartHex(r.bounds.size.y);
        }

       
    }
}
