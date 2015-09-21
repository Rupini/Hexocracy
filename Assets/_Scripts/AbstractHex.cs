using Hexocracy.CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public abstract class AbstractHex : CachedMonoBehaviour, IHex
    {
        public Index2D Index { get; private set; }

        public Index2D[] NeighborsIndexes { get; protected set; }

        

        public virtual void SetIndex(Index2D index)
        {
            Index = index;

            NeighborsIndexes = new Index2D[] 
            { 
                Index.Offset(0,2),
                Index.Offset(1,1),
                Index.Offset(1,-1),
                Index.Offset(0,-2),
                Index.Offset(-1,-1),
                Index.Offset(-1,1)
            };
        }
    }
}
