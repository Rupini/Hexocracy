using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class EmptyContent : IContainable
    {
        private static EmptyContent instance;

        public static EmptyContent Get()
        {
            if (instance == null)
            {
                instance = new EmptyContent();
            }

            return instance;
        }

        private EmptyContent() { }

        public int EntityID { get; private set; }

        public bool Destroyed { get { return false; } }

        public float Height { get { return 0; } }

        public Player Owner { get { return Player.NeutralPassive; } }

        public ContentType Type { get { return ContentType.Empty; } }

        public void Destroy() { OnDestroy(this); }

        public event Action<IEntity> OnDestroy = delegate { };
    }
}
