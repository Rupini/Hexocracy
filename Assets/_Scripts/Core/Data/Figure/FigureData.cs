using Hexocracy.CustomEditor;
using System;
using UnityEngine;

namespace Hexocracy.Core
{
    [Serializable]
    public class FigureData
    {
        public int playerIndex;

        [ReadOnly]
        public string playerName;

        public int actionPoints;
        public int baseHP;
        public int damage;
        public int satiety;
        public int baseMass;

        public int jumpingAngle;
        public int jumpUpHeight;
        public int jumpDownHeight;

        public Color color
        {
            get
            {
               var foundPlayer = PlayerPool.GetPlayers().Find(p => p.index == playerIndex);

               if(foundPlayer != null)
               {
                   return foundPlayer.color;
               }
               else
               {
                   throw new Exception("Cann't find player with index " + playerIndex);
               }
            }
        }

    }
}
