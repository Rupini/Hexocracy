using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Hexocracy.CustomEditor;

namespace Hexocracy
{
    [ExecuteInEditMode]
    public class HexEditor : AbstractHex
    {
        [ReadOnly]
        public int xIndex, yIndex;
        [Range(1,10)]
        public int height = 1;
        
        private int lastHeight = 1;
        private float baseScale;

        public override void SetIndex(Index2D index)
        {
            base.SetIndex(index);
            xIndex = Index.X;
            yIndex = Index.Y;
        }

        public void Initialize(Index2D index, Transform parent)
        {
            SetIndex(index);
            transform.SetParent(parent);
        }

        private void Update()
        {
            ChangeHeight();
        }

        private void ChangeHeight()
        {
            if (lastHeight != height)
            {
                lastHeight = height;
                t.localScale = new Vector3(baseScale, height * baseScale, baseScale);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            baseScale = t.localScale.x;
        }
    }
}
