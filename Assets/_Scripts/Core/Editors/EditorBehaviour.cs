using UnityEngine;

namespace Hexocracy.Core
{
    [ExecuteInEditMode]
    public abstract class EditorBehaviour<T> : CachedMonoBehaviour, IEditorBehaviour<T>
    {
        protected bool gameInstanceInited;

        protected abstract T OnGameInstanceInit();

        public void InitGameInstance()
        {
            ToGameInstance();
        }

        public T ToGameInstance()
        {
            if(!gameInstanceInited)
            {
                gameInstanceInited = true;
                return OnGameInstanceInit();
            }

            return default(T);
        }
    }
}
