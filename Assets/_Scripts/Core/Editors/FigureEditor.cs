using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using URandom = UnityEngine.Random;

namespace Hexocracy.Core
{
    [ExecuteInEditMode]
    public class FigureEditor : EditorBehaviour
    {
        public FigureData data;

        private int previusOwner;

        private void Start()
        {
            if (r.sharedMaterial == null)
            {
                r.sharedMaterial = new Material(Resources.Load<Material>("Models/Materials/editor_whiteMat"));
            }

            data.color = r.sharedMaterial.color;
        }

        private void Update()
        {
            if (previusOwner != data.owner)
            {
                previusOwner = data.owner;
                ChangeColor();
            }
        }

        private void ChangeColor()
        {
            if (previusOwner == 0)
                data.color = new Color(1, 0, 0);
            else if (previusOwner == 1)
                data.color = new Color(0, 1, 0);

            r.sharedMaterial.color = data.color;
        }

        public override void ToGameInstance()
        {
            FigureFactory.I.Create(go, data);
            GameObject.Destroy(this);
        }
    }
}
