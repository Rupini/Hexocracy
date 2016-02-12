using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public abstract class BouncingObject : MapObject
    {
        private FigureContainer container;

        private float angle;
        private float g;
        private PathFinder pathFinder;

        private Rigidbody body;

        private bool forced;
        private bool inRetreatPassing;
        private bool inFlight;

        private Hex previousHex;
        private Hex nextHex;

        private Path currPath;

        protected int ownerPlayerIndex;

        public abstract int AP { get; protected set; }

        #region Initialize
        protected override void Awake()
        {
            base.Awake();
            g = -Physics.gravity.y;
        }

        public virtual void Initialize(FigureContainer container, FigureData data)
        {
            this.container = container;

            angle = data.jumpingAngle * Mathf.Deg2Rad;
            pathFinder = new DijkstraPathFinder(data.jumpUpHeight, data.jumpDownHeight);
            body = GetComponent<Rigidbody>();
            Height = GetComponent<Collider>().bounds.size.y;

            DefineStartHex(0.5f * r.bounds.size.y);

            container.OnFigureCreated((Figure)this);
        }

        #endregion
        #region MoveCommand

        public MoveResult MoveTo(Hex hex, bool forced)
        {
            if (inFlight) return MoveResult.AlreadyInFlight;
            if (AP <= 0) return MoveResult.NotEnoughActionPoints;
            if (Owner == hex.Content.Owner) return MoveResult.BadDestination;

            this.forced = forced;
            currPath = GetPath(hex);

            if (currPath.Useful)
            {
                if (currPath.TotalCost > AP)
                    return MoveResult.NotEnoughActionPoints;
            }
            else
                return MoveResult.Impassable;

            ExecuteMove();
            return MoveResult.Ok;
        }

        private Path GetPath(Hex destination = null, Func<Hex, PassibilityType> passibilityCondition = null, Func<Hex, bool> targetCondition = null)
        {
            Path path;
            if (targetCondition == null)
                path = pathFinder.FindPathToDestination(currentHex, destination, forced, passibilityCondition);
            else
                path = pathFinder.FindPathWithCondition(currentHex, targetCondition, forced, passibilityCondition);

            return path;
        }

        private void MoveToNearestEmptyHex()
        {
            currPath = GetPath(
                 passibilityCondition: (hex) =>
                {
                    if (hex.Content.Owner.IsEnemy(Owner))
                        return PassibilityType.AbsoluteObstacle;
                    else
                        return PassibilityType.Ok;
                },
                targetCondition: (hex) => { return !hex.HasFigure; });

            if (currPath.Useful)
                ExecuteMove();
            else
            {
                Debug.Log("oops!");
                Destroy();
            }
        }

        private bool MoveToPreviusHex()
        {
            currPath = GetPath(previousHex);
            if (currPath.Useful && currPath.Count == 1)
            {
                ExecuteMove();
                return true;
            }
            else
                return false;
        }

        private void ExecuteMove()
        {
            if (currPath.Next()) JumpToNext(currPath.CurrentHex);
        }
        #endregion
        #region JumpControl
        private void JumpToNext(Hex hex)
        {
            previousHex = currentHex;
            nextHex = hex;

            var p1 = t.position;
            var p2 = GetJumpPoint(hex);

            float dt;
            var v = GetVelocity(p1, p2, out dt);

            body.AddForceAtPosition(v, p1, ForceMode.VelocityChange);
            StartJump(dt);
        }

        private void StartJump(float dt)
        {
            LeaveHex();
            AP -= currPath.CurrentCost;

            if (AP < 0) OnForcedMovePenalti();

            inFlight = true;
            StartCoroutine(FinishJump(dt));
        }

        private IEnumerator FinishJump(float dt)
        {
            yield return new WaitForSeconds(dt);

            inFlight = false;
            currentHex = nextHex;

            int bounceHeight = currentHex.H - previousHex.H;

            //Stop motion and centrate
            t.position = GetJumpPoint(currentHex);
            body.velocity = new Vector3();
            //_

            var content = currentHex.Content;
            switch (content.Type)
            {
                case ContentType.Empty:
                    LandedOnHex(bounceHeight);
                    break;
                case ContentType.Figure:
                    OnFigureCollided((Figure)content, bounceHeight, forced);

                    if (content.Destroyed)
                    {
                        bounceHeight = 0;
                        LandedOnHex(bounceHeight);
                    }
                    else
                    {
                        if (!inRetreatPassing)
                            if (forced)
                            {
                                inRetreatPassing = true;
                                MoveToNearestEmptyHex();
                                yield break;
                            }
                            else
                            {
                                if (content.Owner.IsEnemy(Owner))
                                {
                                    forced = true;
                                    if (!MoveToPreviusHex())
                                    {
                                        inRetreatPassing = true;
                                        MoveToNearestEmptyHex();
                                        yield break;
                                    }
                                }
                            }
                    }
                    break;
                case ContentType.Item:
                    OnItemBoxContact(currentHex.Content);
                    LandedOnHex(bounceHeight);
                    break;
            }

            if (currPath.Useful) ExecuteMove();
        }

        private void LandedOnHex(int bounceHeight)
        {
            forced = false;
            inRetreatPassing = false;

            EnterHex();
            OnHexLanded(currentHex, bounceHeight);
        }
        #endregion
        #region Kinemacics

        private Vector3 GetVelocity(Vector3 p1, Vector3 p2, out float t)
        {
            Vector3 r = p2 - p1;

            float dy = p2.y - p1.y;

            float dx = Mathf.Sqrt(r.x * r.x + r.z * r.z);

            t = Mathf.Sqrt(2 * (dx * Mathf.Tan(angle) - dy) / g);

            float v0 = dx / (t * Mathf.Cos(angle));

            r = new Vector3(r.x, dx * Mathf.Tan(angle), r.z);

            return r.normalized * v0;
        }

        private Vector3 GetVelocity(float height, out float t)
        {
            float vy = Mathf.Sqrt(2 * height * g);
            t = 2 * vy / g;

            return new Vector3(0, vy, 0);
        }

        private Vector3 GetJumpPoint(Hex hex)
        {
            Vector3 jPoint = hex.GroundCenter;
            jPoint.y += Height * 0.5f;
            switch (hex.Content.Type)
            {
                case ContentType.Figure:
                    jPoint.y += hex.Content.Height;
                    break;
            }

            return jPoint;
        }

        #endregion
        #region Callbacks
        protected abstract void OnForcedMovePenalti();

        protected abstract void OnFigureCollided(Figure figure, int bounceHeight, bool forced);

        protected abstract void OnHexLanded(Hex hex, int bounceHeight);

       

        protected abstract void OnItemBoxContact(IContainable item);
        #endregion
        #region IContainable

        public override ContentType Type { get { return ContentType.Figure; } }
       
        #endregion
    }
}
