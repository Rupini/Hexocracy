﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class HexAddition : IHexAddition
    {
        public Hex owner { get; protected set; }

        public void Attach(Hex target)
        {
            owner = target;
            OnAttach(owner);
        }

        public void Disattach()
        {
            owner = null;
            OnDisattach();
        }

        protected abstract void OnAttach(Hex target);

        protected abstract void OnDisattach();
       
    }
}
