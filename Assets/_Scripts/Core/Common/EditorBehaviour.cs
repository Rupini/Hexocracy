using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public abstract class EditorBehaviour : CachedMonoBehaviour
    {
        public abstract void ToGameInstance();
    }
}
