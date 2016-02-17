using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class TurnListenerBehaviour : CachedMonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            TurnController.TurnStarted += OnTurnStarted;
            TurnController.TurnFinished += OnTurnFinished;
        }

        protected virtual void OnTurnStarted(bool newRound) { }

        protected virtual void OnTurnFinished(bool roundFinished) { }
    }
}
