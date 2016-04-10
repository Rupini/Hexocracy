using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Hexocracy.CustomEditor;
using Hexocracy.Systems;

namespace Hexocracy.Core
{
    [ExecuteInEditMode]
    public class HexEditor : EditorBehaviour<Hex>
    {
        public HexData data;

        private int lastHeight = 1;
        private bool lastHasAddition;
        private Material defaultMaterial;
        private Material additionMaterial;

        public Index2D Index { get; private set; }

        private void SetIndex(Index2D index)
        {
            Index = index;
            data.xIndex = index.X;
            data.yIndex = index.Y;
        }

        public void Initialize(Index2D index, Transform parent)
        {
            SetIndex(index);
            transform.SetParent(parent);
        }

        private void Update()
        {
            ChangeHeight();
            ChangeAdditionStatus();
        }

        private void ChangeHeight()
        {
            if (lastHeight != data.height)
            {
                lastHeight = data.height;
                t.localScale = new Vector3(HexInfo.Scale.x, data.height * HexInfo.Scale.y, HexInfo.Scale.z);

                var position = t.position;
                position.y = 0.5f * (r.bounds.size.y - HexInfo.H);
                t.position = position;

                data.scaleY = t.localScale.y;
            }
        }

        private void ChangeAdditionStatus()
        {
            if (data.hasAddition != lastHasAddition)
            {
                lastHasAddition = data.hasAddition;

                if (data.hasAddition)
                {
                    r.material = additionMaterial;
                }
                else
                {
                    r.material = defaultMaterial;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            data.scaleY = t.localScale.y;
            data.height = (int)(t.localScale.y / HexInfo.Scale.y);

            defaultMaterial = new Material(RM.LoadMaterial("hexDefaultMat"));
            additionMaterial = new Material(RM.LoadMaterial("hexAdditionMat"));

            lastHasAddition = data.hasAddition;
        }

        protected override Hex OnGameInstanceInit()
        {
            var hex = go.AddComponent<Hex>();
            hex.Initialize(data);
            Destroy(this);
            return hex;
        }

    }
}
