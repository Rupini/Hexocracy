using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class GameMap : Map
    {
        private static GameMap _i;
        public static GameMap I
        {
            get
            {
                if (_i == null) _i = new GameMap();
                return _i;
            }
        }

        public void ResetFlags(int undefinedValue)
        {
            foreach (var pair in hexes)
            {
                pair.Value.FindFlag = undefinedValue;
                pair.Value.ChangeColor(Color.white);
            }
        }
    }
}
