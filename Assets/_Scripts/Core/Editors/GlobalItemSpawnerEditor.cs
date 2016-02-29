using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class GlobalItemSpawnerEditor : EditorBehaviour<GlobalItemSpawner>
    {
        public GlobalItemSpawnerData data;

        protected override GlobalItemSpawner OnGameInstanceInit()
        {
            var itemSpawner = go.AddComponent<GlobalItemSpawner>();
            itemSpawner.Initialize(data);
            Destroy(this);
            return itemSpawner;
        }
      
    }
}
