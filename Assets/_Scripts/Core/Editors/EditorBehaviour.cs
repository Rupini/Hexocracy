using UnityEngine;

namespace Hexocracy.Core
{
    [ExecuteInEditMode]
    public abstract class EditorBehaviour : CachedMonoBehaviour, IEditorBehaviour
    {
        public abstract void ToGameInstance();
    }
}
