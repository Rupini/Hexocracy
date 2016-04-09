using URandom = UnityEngine.Random;

namespace Hexocracy.Core
{
    public class ElementSpawner : HexAddition
    {
        private int capacity;
        private ElementKind kind;
        private int minRespTime;
        private int maxRespTime;

        private bool respawnOnStart;

        private int roundCounter;
        private int requiredRound;

        public ElementSpawner(HexData.Addition data)
        {
            capacity = data.capacity;
            kind = data.kind;
            
            minRespTime = data.minRespawnTime;
            maxRespTime = data.maxRespawnTime;
            
            respawnOnStart = data.respawnOnStart;

            roundCounter = 0;
            requiredRound = URandom.Range(minRespTime, maxRespTime + 1);
        }

        public override void OnTurnUpdate(bool isNewRound)
        {
            if (isNewRound)
            {
                if (respawnOnStart || roundCounter == requiredRound)
                {
                    respawnOnStart = false;
                    roundCounter = 0;
                    requiredRound = URandom.Range(minRespTime, maxRespTime + 1);
                    Spawn();
                }
                else
                {
                    roundCounter++;
                }
            }
        }

        private void Spawn()
        {
            if (owner.Content.Type == ContentType.Empty)
            {
                var data = new ElementData()
                {
                    capacity = this.capacity,
                    kind = this.kind
                };

                GS.Get<ItemFactory>().Create(data, owner);
            }
        }
      
    }
}
