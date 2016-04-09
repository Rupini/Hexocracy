using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [RawPrototype]
    [GameService(GameServiceType.Container)]
    public class ActorContainer : EntityContainer<IActor>
    {
        private ActorContainer() { }
       
        private void r_post_ctor()
        {
            GS.Get<FigureContainer>().OnContentInitialized += OnActorAdded;
            GS.Get<FigureContainer>().OnAdd += OnActorAdded;
            GS.Get<FigureContainer>().OnRemove += OnActorRemoved;
        }

        private void OnActorAdded(IEnumerable<Figure> actors)
        {
            Add(actors.Cast<IActor>());
        }

        private void OnActorRemoved(IEnumerable<Figure> actors)
        {
            Remove(actors.Cast<IActor>());
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            GS.Get<FigureContainer>().OnContentInitialized -= OnActorAdded;
            GS.Get<FigureContainer>().OnAdd -= OnActorAdded;
            GS.Get<FigureContainer>().OnRemove -= OnActorRemoved;
        }
    }
}
