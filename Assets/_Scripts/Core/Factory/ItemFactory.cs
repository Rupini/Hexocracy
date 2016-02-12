using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Hexocracy.Core
{
    public class ItemFactory
    {
        private static ItemFactory _i;
        public static ItemFactory I
        {
            get
            {
                if (_i == null)
                {
                    _i = new ItemFactory();
                }
                return _i;
            }
        }

        public ItemBox Create(ItemData data, Hex hex)
        {
            return Create(data, hex, Player.NeutralPassive);
        }

        public ItemBox Create(ItemData data, Hex hex, Player owner)
        {
            ItemBox itemBox;

            if (data.type == ItemType.Element)
            {
                itemBox = GameObject.Instantiate(Resources.Load<ItemBox>("Prefabs/Play/Element"));

                if (data.kind == ElementKind.Random || data.kind == ElementKind.None)
                {
                    data.kind = (ElementKind)URandom.Range(1, 4);
                }

                switch (data.kind)
                {
                    case ElementKind.Red:
                        data.color = new Color(1, 0, 0);
                        break;
                    case ElementKind.Green:
                        data.color = new Color(0, 1, 0);
                        break;
                    case ElementKind.Blue:
                        data.color = new Color(0, 0, 1);
                        break;
                }

            }
            else if (data.type == ItemType.Other)
            {
                itemBox = GameObject.Instantiate(Resources.Load<ItemBox>("Prefabs/Play/Bomb"));

                if (owner.Index == 0)
                    data.color = new Color(1, 0, 0);
                else
                    data.color = new Color(0, 1, 0);
            }
            else
            {
                itemBox = null;
            }

            var position = hex.GroundCenter;
            position.y += 2;
            itemBox.t.position = position;

            itemBox.Initialize(owner, data);

            return itemBox;
        }
    }
}
