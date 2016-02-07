using Hexocracy.Mech;
using System;
using System.Collections.Generic;

namespace Hexocracy.Core
{
    public class LiPathFinder : PathFinder
    {
        private const int OBSTACLE_FLAG = -1;
        private const int UNDEFINED_FlAG = 0;
        private const int ORIGIN_FLAG = 1;

        public LiPathFinder(int jumpUpHeight, int jumpDownHeight)
            : base(jumpUpHeight, jumpDownHeight)
        {

        }

        protected override Path FindOriginalPath(Hex origin, Hex destination = null, bool forced = false, Func<Hex, PassibilityType> checkPassibiliy = null, Func<Hex, bool> isTarget = null)
        {
            GameMap.I.ResetFlags(UNDEFINED_FlAG);

            origin.FindFlag = ORIGIN_FLAG;

            if (Find(new List<Hex>() { origin }))
                return BuildPath();
            else
                return new Path();
        }

        private bool Find(List<Hex> hexes)
        {
            List<Hex> findingHexes = new List<Hex>();

            foreach (var hex in hexes)
            {
                foreach (var neighbor in hex.Neighbors)
                {
                    if (neighbor.FindFlag == UNDEFINED_FlAG && CheckHeight(hex, neighbor, forced))
                    {
                        if (!isTarget(neighbor))
                        {
                            switch (checkPassibility(neighbor))
                            {
                                case PassibilityType.Ok:
                                    neighbor.FindFlag = hex.FindFlag + 1;
                                    findingHexes.Add(neighbor);
                                    break;
                                case PassibilityType.AbsoluteObstacle:
                                    neighbor.FindFlag = OBSTACLE_FLAG;
                                    break;
                            }
                        }
                        else
                        {
                            destination = neighbor;
                            destination.FindFlag = hex.FindFlag + 1;
                            return true;
                        }
                    }
                }
            }

            if (findingHexes.Count != 0)
                return Find(findingHexes);
            else
                return false;
        }

        private Path BuildPath()
        {
            List<Hex> hexes = new List<Hex>();
            var hex = destination;

            while (hex != null)
            {
                hexes.Add(hex);
                hex = hex.Neighbors.Find((h) => { return h.FindFlag == hex.FindFlag - 1 && CheckHeight(hex, h, forced); });
            }

            hexes.Reverse();

            return new Path(hexes, 1);
        }
    }
}
