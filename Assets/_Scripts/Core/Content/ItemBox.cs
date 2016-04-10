﻿using Hexocracy.Core;
using Hexocracy.Systems;
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

        public Item Item { get; protected set; }

        protected int lifeTime;
        protected bool hasLifeTime;

        private ItemBox()
        {
            Height = 0;
            Owner = Player.NeutralPassive;
        }

        public void Initialize(Player owner, ItemData data)
        {
            Owner = owner;

            Item = new Element(this, (ElementData)data);

            lifeTime = data.lifeTime;
            hasLifeTime = 0 != lifeTime;
          
            r.material.color = data.color;

            var yOffset = !data.overrideOffsetK ? r.bounds.size.y * 0.5f : r.bounds.size.y * data.yOffsetK;

            DefineStartHex(yOffset);
        }

        public virtual void Contact(Figure figure)
        {
            Item.Contact(figure);
        }

        protected override void OnTurnFinished(bool roundFinished)
        {
            if (roundFinished)
            {
                if (hasLifeTime)
                {
                    lifeTime--;
                    if (lifeTime == 0)
                    {
                        Destroy();
                    }
                }
            }
        }

    }
}
