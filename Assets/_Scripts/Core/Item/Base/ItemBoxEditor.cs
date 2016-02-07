using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    [ExecuteInEditMode]
    public class ItemBoxEditor : EditorBehaviour
    {
        public ItemData data;

        private ElementKind previusKind;

        private void Start()
        {
            if (!Application.isPlaying)
            {
                if (r.sharedMaterial == null)
                    r.sharedMaterial = new Material(Resources.Load<Material>("Models/Materials/editor_whiteColor"));
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

        public override void ToGameInstance()
        {
            go.AddComponent<ItemBox>().Initialize(data);
            Destroy(this); 
        }
    }
}
