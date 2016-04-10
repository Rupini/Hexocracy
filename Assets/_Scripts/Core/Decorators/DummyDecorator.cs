using Hexocracy.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class DummyDecorator : IDecorator
    {
        private Color defaultColor;
        private Color activateColor;
        
        private MeshRenderer renderer;

        private bool defState;

        public DummyDecorator(GameObject decoratedObject, Color defaultColor, Color activateColor)
        {
            this.defaultColor = defaultColor;
            this.activateColor = activateColor;

            renderer = decoratedObject.GetComponent<MeshRenderer>();

            renderer.material.color = defaultColor;

            defState = true;
        }

        //public void SetTexture(string textureName)
        //{
        //    renderer.material.mainTexture = RM.LoadTexture(textureName);
        //}

        //public void SetMaterial(string materialName)
        //{
        //    renderer.material = RM.LoadMaterial(materialName);
        //}

        public void SwitchState()
        {
            if (!renderer) return;

            defState = !defState;

            if (defState)
            {
                renderer.material.color = defaultColor;
            }
            else
            {
                renderer.material.color = activateColor;
            }
        }
    }
}
