using Hexocracy.CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    [Serializable]
    public class PlayerData
    {
        public int index;

        public string playerName;
        public List<int> enemies;
        public List<int> allies;

        public Color color;

        
    }
}
