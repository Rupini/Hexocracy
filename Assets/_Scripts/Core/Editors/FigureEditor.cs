using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using URandom = UnityEngine.Random;
using Hexocracy.Systems;

namespace Hexocracy.Core
{
    [RawPrototype]
    public class FigureEditor : EditorBehaviour<Figure>
    {
        public FigureData data;

        private int previusOwner;

        private void Start()
        {
            if (r.sharedMaterial == null)
            {
                r.sharedMaterial = new Material(RM.LoadMaterial("editor_whiteMat"));
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

        protected override Figure OnGameInstanceInit()
        {
            GS.Get<FigureFactory>().Create(go, data);
            
            GameObject.Destroy(this);
            
            return go.GetComponent<Figure>();
        }
    }
}
