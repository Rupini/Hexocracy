using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [RequireComponent(typeof(Figure))]
    public class Actor : TurnListenerBehaviour, IActor
    {
        private Color defaultColor;

        #region IActor
        public int EntityID { get; protected set; }

        public IDecorator Decorator { get; protected set; }

        public bool Active { get; protected set; }

        public Player Owner { get; protected set; }

        public bool Destroyed { get; protected set; }

        public Vector3 Position { get { return t.position; } }

        public Figure Figure { get; protected set; }

        public event Action<IEntity> OnDestroy = delegate { };
        #endregion

        protected override void Awake()
        {
            base.Awake();

            Figure = GetComponent<Figure>();

            EntityID = Figure.EntityID;
            Owner = Figure.Owner;
            Decorator = Figure.Decorator;

            Figure.OnDestroy += (e) => Destroy();
        }

        public bool Activate()
        {
            Active = true;
            Decorator.SwitchState();

            return true;
        }

        public bool Deactivate()
        {
            if (!Figure.InFlight)
            {
                Active = false;
                Decorator.SwitchState();

                return true;
            }

            return false;
        }

        public override void Destroy()
        {
            base.Destroy();

            Destroyed = true;
            OnDestroy(this);
            Destroy(go);
        }
    }
}
