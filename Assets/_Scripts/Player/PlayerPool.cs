using Hexocracy.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [DestroyAfterStart]
    public class PlayerPool : MonoBehaviour
    {
        private static PlayerPool instance;

        public static List<PlayerData> GetPlayers()
        {
            if(!instance)
            {
               instance = FindObjectOfType<PlayerPool>();

                if(!instance)
                {
                    instance = RM.InstantiateEditorPrefab<PlayerPool>("PlayerPoolDefault");
                }
            }

            return instance.players;
        }

        public List<PlayerData> players;
    }
}
