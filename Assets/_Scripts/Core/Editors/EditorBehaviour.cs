using UnityEngine;

namespace Hexocracy.Core
{
    [ExecuteInEditMode]
    public abstract class EditorBehaviour<T> : CachedMonoBehaviour, IEditorBehaviour<T>
    {
        public void InitGameInstance()
        {
            ToGameInstance();
        }
        public abstract T ToGameInstance();
    }
}
