using Hexocracy.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Hexocracy.Core
{
    [RawPrototype]
    [GameService(GameServiceType.Factory)]
    public class ItemFactory
    {
        private ItemFactory() { }

        public ItemBox Create(ItemData data, Hex hex)
        {
            return Create(data, hex, Player.NeutralPassive);
        }

        public ItemBox Create(ItemData data, Hex hex, Player owner)
        {
            var itemBox = RM.InstantiatePrefab<ItemBox>("Element");

            if (data.type == ItemType.Element)
            {
                var elementData = (ElementData)data;

                if (elementData.kind == ElementKind.Random || elementData.kind == ElementKind.None)
                {
                    elementData.kind = (ElementKind)URandom.Range(1, 4);
                }

                switch (elementData.kind)
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

            var position = hex.GroundCenter;
            position.y += itemBox.r.bounds.size.y;
            itemBox.t.position = position;

            itemBox.Initialize(owner, data);

            return itemBox;
        }
    }
}
