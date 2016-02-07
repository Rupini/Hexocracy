using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public abstract class ActivableObject : MapObject
    {
        public bool Active { get; protected set; }
        private Color defaultColor;

        public void Deactivate()
        {
            Active = false;
            Owner.SwitchActiveState(false);
            if (this)//Cruntch!
            {
                r.material.color = defaultColor;
            }
        }

        public void Activate()
        {
            Active = true;
            Owner.SwitchActiveState(true);
            defaultColor = r.material.color;
            r.material.color = new Color(1, 1, 1);
        }

    }
}
