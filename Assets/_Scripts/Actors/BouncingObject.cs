﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public abstract class BouncingObject : CachedMonoBehaviour, IContainable
    {
        public int ownerPlayerIndex;
        public int baseMovePoints;

        public abstract int MP { get; protected set; }
        [Range(30, 75)]
        public float angleInDegree;
        public int jumpUpHeight;
        public int jumpDownHeight;

        private float angle { get { return angleInDegree * Mathf.Deg2Rad; } }
        private float g;
        private PathFinder pathFinder;

        private Rigidbody body;

        private bool forced;
        private bool inRetreatPassing;
        private bool inFlight;

        private Hex currentHex;
        private Hex previousHex;
        private Hex nextHex;

        private Path currPath;

        #region Initialize
        protected override void Awake()
        {
            base.Awake();
            //pathFinder = new LiPathFinder(jumpUpHeight, jumpDownHeight);
            pathFinder = new DijkstraPathFinder(jumpUpHeight, jumpDownHeight);
            g = -Physics.gravity.y;
            body = GetComponent<Rigidbody>();
            Height = GetComponent<Collider>().bounds.size.y;

            TurnController.RoundStarted += OnRoundStarted;
            TurnController.RoundFinished += OnRoundFinished;
        }

        protected virtual void Start()
        {
            DefineStartHex();

            Owner = Player.GetByIndex(ownerPlayerIndex);

            if (ownerPlayerIndex == 0)
                r.material.mainTexture = Resources.Load<Texture>("Models/redColor");
            else
                r.material.mainTexture = Resources.Load<Texture>("Models/greenColor");
        }

        private void DefineStartHex()
        {
            RaycastHit hit;
            if (Physics.Raycast(t.position, new Vector3(0, -1, 0), out hit, 10, 1 << 8))
            {
                currentHex = hit.collider.GetComponent<Hex>();
                currentHex.OnContentAppeared(this);

                t.position = currentHex.GroundCenter;
            }
            else
                Debug.LogError("Can't define start hex!");
        }
        #endregion
        #region MoveCommand
        public MoveResult MoveTo(Hex hex, bool forced)
        {
            if (inFlight) return MoveResult.AlreadyInFlight;
            if (MP <= 0) return MoveResult.NotEnoughMovePoints;
            if (Owner == hex.Content.Owner) return MoveResult.BadDestination;

            this.forced = forced;
            currPath = GetPath(hex);

            if (currPath.Useful)
            {
                if (currPath.Count > MP)
                    return MoveResult.NotEnoughMovePoints;
            }
            else
                return MoveResult.Impassable;

            ExecuteMove();
            return MoveResult.Ok;
        }

        private Path GetPath(Hex destination = null, Func<Hex, LiPathFinder.PassibilityType> passibilityCondition = null, Func<Hex, bool> targetCondition = null)
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
                        return LiPathFinder.PassibilityType.AbsoluteObstacle;
                    else
                        return LiPathFinder.PassibilityType.Ok;
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
            currentHex.OnContentMissed(this);
            MP -= currPath.CurrentCost;

            if (MP < 0) OnForcedMovePenalti();

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
                                    if(!MoveToPreviusHex())
                                    {
                                        inRetreatPassing = true;
                                        MoveToNearestEmptyHex();
                                        yield break;
                                    }
                                }
                            }
                    }
                    break;
                case ContentType.Element:

                    break;
            }

            if (currPath.Useful) ExecuteMove();
        }

        private void LandedOnHex(int bounceHeight)
        {
            forced = false;
            inRetreatPassing = false;

            currentHex.OnContentAppeared(this);
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

        protected abstract void OnRoundStarted(bool newCicle);

        protected abstract void OnRoundFinished();
        #endregion
        #region IContainable

        public bool Destroyed { get; private set; }

        public float Height { get; private set; }

        public Player Owner { get; private set; }

        public ContentType Type { get { return ContentType.Figure; } }

        public void Destroy()
        {
            Destroyed = true;
            currentHex.OnContentMissed(this);

            TurnController.RoundStarted -= OnRoundStarted;
            TurnController.RoundFinished -= OnRoundFinished;

            Destroy(go);
        }
        #endregion
    }
}
