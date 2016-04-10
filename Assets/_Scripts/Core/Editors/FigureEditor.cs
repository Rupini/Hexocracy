using UnityEngine;
using Hexocracy.Systems;

namespace Hexocracy.Core
{
    public class FigureEditor : EditorBehaviour<Figure>
    {
        public FigureData data;

        private Color prevColor;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (data.color != prevColor)
                {
                    prevColor = data.color;
                    
                    r.sharedMaterial = new Material(RM.LoadMaterial("editor_whiteMat"));
                    r.sharedMaterial.color = prevColor;

                    var player = PlayerPool.GetPlayers().Find(p => p.index == data.playerIndex);
                    data.playerName = player != null ? player.playerName : "bad index!";
                }
            }
        }

        protected override Figure OnGameInstanceInit()
        {
            GS.Get<FigureFactory>().Create(go, data);

            Destroy(this);

            return go.GetComponent<Figure>();
        }
    }
}
