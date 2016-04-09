using UnityEngine;

namespace Hexocracy.Core
{
    [ExecuteInEditMode]
    public abstract class EditorBehaviour<T> : CachedMonoBehaviour, IEditorBehaviour<T> where T : IEntity
    {
        protected bool gameInstanceInited;

        protected abstract T OnGameInstanceInit();

        public T ToGameInstance()
        {
            if (!gameInstanceInited)
            {
                gameInstanceInited = true;
                GameInstance = OnGameInstanceInit();
                return GameInstance;
            }

            return default(T);
        }

        public IEntity ToGameEntity()
        {
            return ToGameInstance();
        }

        public T GameInstance { get; protected set; }

    }
}
