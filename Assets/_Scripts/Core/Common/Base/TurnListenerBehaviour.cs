using Hexocracy.Systems;

namespace Hexocracy.Core
{
    public abstract class TurnListenerBehaviour : CachedMonoBehaviour
    {
        public virtual void Destroy()
        {
            GS.Get<TurnController>().TurnStarted -= OnTurnStarted;
            GS.Get<TurnController>().TurnFinished -= OnTurnFinished;
        }

        protected override void Awake()
        {
            base.Awake();
            GS.Get<TurnController>().TurnStarted += OnTurnStarted;
            GS.Get<TurnController>().TurnFinished += OnTurnFinished;
        }

        protected virtual void OnTurnStarted(bool isNewRound) { }

        protected virtual void OnTurnFinished(bool roundFinished) { }
    }
}
