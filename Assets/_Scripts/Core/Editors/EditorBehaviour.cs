using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class EditorBehaviour : CachedMonoBehaviour, IEditorBehaviour
    {
        public abstract void ToGameInstance();
    }
}
