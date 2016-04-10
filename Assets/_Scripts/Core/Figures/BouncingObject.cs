using Hexocracy.HelpTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{

    [RawPrototype]
    public abstract class BouncingObject : MapObject
    {
        #region Defenition
        private float angle;
        private PathFinder pathFinder;

        private Rigidbody body;

        private bool forced;
        private bool inRetreatPassing;
        private bool inFlight;

        private Hex previousHex;
        private Hex nextHex;

        private Path currPath;

        protected int ownerPlayerIndex;
        protected int jumpUpHeight;
        protected int jumpDownHeight;

        public abstract float AP { get; protected set; }

        #endregion

        #region Initialize
        protected override void Awake()
        {
            base.Awake();
        }

        public virtual void Initialize(FigureData data)
        {
            jumpUpHeight = data.jumpUpHeight;
            jumpDownHeight = data.jumpDownHeight;

            angle = data.jumpingAngle * Mathf.Deg2Rad;
            pathFinder = new DijkstraPathFinder(jumpUpHeight, jumpDownHeight);
            body = GetComponent<Rigidbody>();
            Height = r.bounds.size.y;

            DefineStartHex(0.5f * r.bounds.size.y);

            
        }

        #endregion

        #region MoveCommand

        public MoveResult MoveTo(Hex hex, bool forced)
        {
            if (inFlight) return MoveResult.AlreadyInFlight;
            if (AP <= 0) return MoveResult.NotEnoughActionPoints;
            if (hex.HasFigure && Owner == hex.Content.Owner) return MoveResult.BadDestination;

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
            {
                ExecuteMove();
            }
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
            var v = Kinematics.CalculateVelocity(p1, p2, angle, out dt);

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
                    OnContentContact(currentHex.Content);
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

        protected abstract void OnContentContact(IContainable item);
        #endregion

        #region IContainable

        public override ContentType Type { get { return ContentType.Figure; } }

        #endregion
    }
}
