using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class GlobalItemSpawnerEditor : EditorBehaviour
    {
        public GlobalItemSpawnerData data;

        public override void ToGameInstance()
        {
            go.AddComponent<GlobalItemSpawner>().Initialize(data);
            Destroy(this);
        }

        private void Update()
        {
        }
    }
}
