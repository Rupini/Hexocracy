using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    public class Element : CachedMonoBehaviour, IContainable
    {
        #region IContainable
        public float Height { get { return 0; } }
        
        public ContentType Type { get { return ContentType.Element; } }
        
        public bool Destroyed { get; private set; }

        public Player Owner { get { return Player.NeutralPassive; } }

        public void Destroy()
        {
            Destroyed = true;
            Destroy(go);
        }
        #endregion
    }
}
