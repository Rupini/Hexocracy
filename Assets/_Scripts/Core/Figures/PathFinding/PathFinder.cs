using Hexocracy.Mech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public abstract class PathFinder
    {
        protected GameMap map;

        protected Hex destination;
        protected Func<Hex, PassibilityType> checkPassibility;
        protected Func<Hex, bool> isTarget;
        protected int jumpUpHeight;
        protected int jumpDownHeight;
        protected bool forced;

        protected Func<Hex, Hex, int> calculateMovementCost;

        public PathFinder(int jumpUpHeight, int jumpDownHeight, Func<Hex, Hex, int> calculateMovementCost)
        {
            map = GS.Get<GameMap>();
            this.jumpUpHeight = jumpUpHeight;
            this.jumpDownHeight = jumpDownHeight;
            this.calculateMovementCost = calculateMovementCost;
        }

        public Path FindPathToDestination(Hex origin, Hex destination, bool forced, Func<Hex, PassibilityType> checkPassibiliy = null)
        {
            return FindPath(origin, destination: destination, forced: forced, checkPassibiliy: checkPassibiliy);
        }

        public Path FindPathWithCondition(Hex origin, Func<Hex, bool> isTarget, bool forced, Func<Hex, PassibilityType> checkPassibiliy = null)
        {
            return FindPath(origin, forced: forced, checkPassibiliy: checkPassibiliy, isTarget: isTarget);
        }

        protected Path FindPath(Hex origin, Hex destination = null, bool forced = false, Func<Hex, PassibilityType> checkPassibiliy = null, Func<Hex, bool> isTarget = null)
        {
            this.forced = forced;
            this.destination = destination;
            this.checkPassibility = checkPassibiliy == null ? DefaultCheckPassability : checkPassibiliy;
            this.isTarget = isTarget == null ? IsDefaultTarget : isTarget;

            if (destination != null && origin.IsNeighbor(destination) && CheckHeight(origin, destination, forced))
                return FindTrivialPath(origin, destination);
            else
                return FindOriginalPath(origin, destination, forced, checkPassibiliy, isTarget);
        }

        protected Path FindTrivialPath(Hex origin, Hex destination)
        {
            

            return new Path(new List<PathVertex>() { 
                new PathVertex(origin, calculateMovementCost(origin, destination)), 
                new PathVertex(destination, 0) });
        }

        protected abstract Path FindOriginalPath(Hex origin, Hex destination = null, bool forced = false, Func<Hex, PassibilityType> checkPassibiliy = null, Func<Hex, bool> isTarget = null);

        public bool CheckHeight(Hex hex, Hex neighbor, bool forced)
        {
            var dh = neighbor.H - hex.H;

            if (dh == 0)
                return true;
            else if (dh < 0)
                return forced || -dh <= jumpDownHeight;
            else
                return dh <= jumpUpHeight;
        }

        protected PassibilityType DefaultCheckPassability(Hex hex)
        {
            return PassibilityType.Ok;
        }

        protected bool IsDefaultTarget(Hex hex)
        {
            return destination == hex;
        }
    }
}
