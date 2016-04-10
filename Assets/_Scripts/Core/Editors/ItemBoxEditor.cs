using Hexocracy.CustomEditor;
using Hexocracy.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{

    [RawPrototype]
    public class ItemBoxEditor : EditorBehaviour<ItemBox>
    {
        public ItemType type;
        
        [Condition(true,"type","Element")]
        public ElementData data;

        private ElementKind previusKind;

        private void Start()
        {
            if (!Application.isPlaying)
            {
                if (r.sharedMaterial == null)
                    r.sharedMaterial = new Material(RM.LoadMaterial("editor_whiteMat"));
                else
                    r.sharedMaterial = new Material(r.sharedMaterial);
            }
        }

        private void Update()
        {
            if (previusKind != data.kind)
            {
                previusKind = data.kind;
                ChangeColor();
            }
        }

        private void ChangeColor()
        {
            switch (previusKind)
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

            r.sharedMaterial.color = data.color;
        }

        protected override ItemBox OnGameInstanceInit()
        {
            var itemBox = go.AddComponent<ItemBox>();
            itemBox.Initialize(Player.NeutralPassive, data);
            Destroy(this);
            return itemBox;
        }
    }
}
