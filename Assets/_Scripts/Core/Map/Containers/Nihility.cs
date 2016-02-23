using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    [GameService(GameServiceType.Container | GameServiceType.Factory)]
    public class Nihility : EntityContainer<NullHex>
    {
        public const int EXTERNAL_SECTOR_INDEX = 0;

        private GameMap _map;
        private GameMap map
        {
            get
            {
                if (_map == null)
                {
                    _map = GameServices.Get<GameMap>();
                }

                return _map;
            }
        }

        private List<Dictionary<int, NullHex>> sectors;
        private Dictionary<int, NullHex> processingHex;
        private Dictionary<int, NullHex> undefineds;

        private bool externalSectorDefined;

        private Nihility()
        {
            undefineds = new Dictionary<int, NullHex>();
        }

        public NullHex GetDefinedNull(Index2D index)
        {
            var hex = Get(index);
            if(hex == null)
            {
                hex = new NullHex(index);
                Add(hex);
                return hex;
            }
            else
            {
                return hex;
            }
        }

        public NullHex GetUndefinedNull(Index2D index)
        {
            if (undefineds.ContainsKey(index))
            {
                return undefineds[index];
            }
            else
            {
                var hex = new NullHex(index, false);
                undefineds[index] = hex;
                return hex;
            }
        }

        public void ToProcess()
        {
            processingHex = new Dictionary<int, NullHex>(entities);
            sectors = new List<Dictionary<int, NullHex>>();

            while (processingHex.Count > 0)
            {
                BuildSector(processingHex.Values.First());
            }
        }

        public Dictionary<int, NullHex> GetSector(int sectorIndex)
        {
            return sectors[sectorIndex];
        }

        public Dictionary<int, NullHex> GetExternal()
        {
            return sectors[EXTERNAL_SECTOR_INDEX];
        }

        private void BuildSector(NullHex startingHex)
        {
            var hexes = new Dictionary<int, NullHex>() { { startingHex.EntityID, startingHex } };
            var currIteration = new List<NullHex>() { startingHex };
            var nextIteration = new List<NullHex>();

            //RefactoringStart
            while (currIteration.Count > 0)
            {
                foreach (var hex in currIteration)
                {
                    hex.DefineCircum();
                    foreach (var circumHex in hex.Circum)
                    {
                        if (circumHex.IsNihility && circumHex.Defined && !hexes.ContainsKey(circumHex.EntityID))
                        {
                            var nullHex = (NullHex)circumHex;
                            hexes[nullHex.EntityID] = nullHex;
                            nextIteration.Add(nullHex);
                        }
                    }
                }

                currIteration = new List<NullHex>(nextIteration);
                nextIteration.Clear();
            }
            //End
            var nihilityIndex = CheckOnExternal(hexes.Values) ? EXTERNAL_SECTOR_INDEX : sectors.Count;

            foreach (var hex in hexes.Values)
            {
                processingHex.Remove(hex.EntityID);
                hex.SetNihilitySectorIndex(nihilityIndex);
            }

            sectors.Insert(nihilityIndex, hexes);
        }

        private bool CheckOnExternal(IEnumerable<NullHex> hexes)
        {
            if (externalSectorDefined)
                return false;
            else
            {
                bool isExternalSector = false;
                
                foreach (var hex in hexes)
                {
                    if (!map.InMapRange(hex.Index))
                    {
                        isExternalSector = true;
                        break;
                    }
                }
                
                externalSectorDefined = isExternalSector;
                return isExternalSector;
            }
        }
    }
}
