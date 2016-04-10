using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IDecorator
    {
        void SwitchState();

        //void SetTexture(string textureName);

        //void SetMaterial(string materialName);
    }
}
