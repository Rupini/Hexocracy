using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class BombItem : Item, IAttacker
    {
        public float Damage { get; private set; }

        public Player Owner { get { return box.Owner; } }
        

        public BombItem(ItemBox box, BombData data)
            : base(box)
        {
            Damage = data.damage;
        }

        public override ItemType Type
        {
            get
            {
                return ItemType.Other;
            }
        }

        protected override bool OnContact(Figure figure)
        {
            float damage = Owner.IsEnemy(figure.Owner) ? Damage : 0;
            figure.OnAttack(this, damage);
            return true;
        }
    }
}
