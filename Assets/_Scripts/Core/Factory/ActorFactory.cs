using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [GameService(GameServiceType.Factory)]
    public class ActorFactory
    {
        private ActorContainer container;

        private ActorFactory() { }
        private void r_post_ctor()
        {
            container = GS.Get<ActorContainer>();
        }

        public void Create(GameObject baseObject)
        {
            var actor = baseObject.AddComponent<Actor>();
            container.Add(actor);
        }

    }
}
