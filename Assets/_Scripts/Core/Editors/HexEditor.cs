﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Hexocracy.CustomEditor;

namespace Hexocracy.Core
{
    [ExecuteInEditMode]
    public class HexEditor : AbstractHex, IEditorBehaviour
    {
        private const float DEFAULT_BASE_SCALE = 0.005f;

        public HexData data;
        
        private int lastHeight = 1;

        public override void SetIndex(Index2D index)
        {
            base.SetIndex(index);
            data.xIndex = Index.X;
            data.yIndex = Index.Y;
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
            if (lastHeight != data.height)
            {
                lastHeight = data.height;
                t.localScale = new Vector3(DEFAULT_BASE_SCALE, data.height * DEFAULT_BASE_SCALE, DEFAULT_BASE_SCALE);
                data.scaleY = t.localScale.y;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            data.scaleY = t.localScale.y;
            data.height = (int) (t.localScale.y / DEFAULT_BASE_SCALE);
        }

        public void ToGameInstance()
        {
            var hex = go.AddComponent<Hex>();
            hex.Initialize(GameMap.I, data);
            GameMap.I.Add(hex);
            Destroy(this);
        }
    }
}