using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public class NullHex : IHex
    {
        #region IHex

        public bool IsNihility { get { return true; } }

        public Index2D Index { get; private set; }

        public Index2D[] CircumIndices { get; private set; }

        public IHex[] Circum { get; private set; }

        public int EntityID { get { return Index; } }

        #endregion

        public bool Defined { get; private set; }

        public int NihilitySectorIndex { get; private set; }

        public bool InExternalNihility { get { return NihilitySectorIndex == Nihility.EXTERNAL_SECTOR_INDEX; } }

        public NullHex(Index2D index)
            : this(index, true)
        {
        }

        public NullHex(Index2D index, bool defined)
        {
            Defined = defined;
            Circum = new IHex[6];
            Index = index;
            CircumIndices = HexInfo.GetCircumIndices(index);
        }

        public void DefineCircum()
        {
            var nihility = GameServices.Get<Nihility>();
            var map = GameServices.Get<GameMap>();

            for (int i = 0; i < CircumIndices.Length; i++)
            {
                var index = CircumIndices[i];
                IHex hexAround = nihility.Get(index);
                if (hexAround == null)
                {
                    hexAround = map.Get(index);
                    if (hexAround == null)
                    {
                        Circum[i] = nihility.GetUndefinedNull(index);
                        continue;
                    }
                }

                Circum[i] = hexAround;
            }
        }

        public void SetNihilitySectorIndex(int nihilityIndex)
        {
            NihilitySectorIndex = nihilityIndex;
        }
    }
}
