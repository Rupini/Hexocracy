using Hexocracy.CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public abstract class AbstractHex : CachedMonoBehaviour, IHex
    {
        protected AbstractHex()
        {
            Circum = new IHex[6];
        }

        #region IHex

        public bool Defined { get { return true; } }

        public bool IsNihility { get { return false; } }

        public Index2D Index { get; private set; }

        public Index2D[] CircumIndices { get; protected set; }

        public IHex[] Circum { get; protected set; }

        public int EntityID { get { return Index; } }
        #endregion

        public virtual void SetIndex(Index2D index)
        {
            Index = index;

            CircumIndices = HexInfo.GetCircumIndices(index);
        }
    }
}
