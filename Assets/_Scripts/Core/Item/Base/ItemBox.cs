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

            r.material = Resources.Load<Material>("Models/Materials/whiteMat");
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteColor");
            
            switch(data.kind)
            {
                case ElementKind.Red:
                    r.material.color = new Color(1, 0, 0);
                    break;
                case ElementKind.Green:
                    r.material.color = new Color(0, 1, 0);
                    break;
                case ElementKind.Blue:
                    r.material.color = new Color(0, 0, 1);
                    break;
            }

            DefineStartHex(r.bounds.size.y);
        }

       
    }
}
