using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class ActorEditor : EditorBehaviour<Actor>
    {

        protected override Actor OnGameInstanceInit()
        {
            GS.Get<ActorFactory>().Create(gameObject);

            Destroy(this);

            return GetComponent<Actor>();
        }
    }
}
