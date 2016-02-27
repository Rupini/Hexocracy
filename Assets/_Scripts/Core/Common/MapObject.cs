using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public abstract class MapObject : TurnListenerBehaviour, IContainable
    {
        protected Hex currentHex;

        #region IContainable
        public int EntityID { get; protected set; }

        public abstract ContentType Type { get; }

        public float Height { get; protected set; }

        public Player Owner { get; protected set; }

        public bool Destroyed { get; protected set; }

        public virtual void Destroy()
        {
            TurnController.TurnStarted -= OnTurnStarted;
            TurnController.TurnFinished -= OnTurnFinished;

            LeaveHex();

            Destroyed = true;
            OnDestroy();
            Destroy(go);
        }

        public event Action OnDestroy = delegate { };
        #endregion

        protected void DefineStartHex(float groundCenterYOffset)
        {
            RaycastHit hit;
            if (Physics.Raycast(t.position, new Vector3(0, -1, 0), out hit, 10, 1 << 8))
            {
                currentHex = hit.collider.GetComponent<Hex>();
                EnterHex();
                var position = currentHex.GroundCenter;
                position.y += groundCenterYOffset;

                t.position = position;
            }
            else
                Debug.LogError("Can't define start hex!");
        }

        protected virtual void EnterHex()
        {
            currentHex.OnContentEntered(this);
        }

        protected virtual void LeaveHex()
        {
            currentHex.OnContentLeft(this);
        }

    }
}
