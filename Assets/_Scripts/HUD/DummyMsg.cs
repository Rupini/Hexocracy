using Hexocracy.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Hexocracy.HUD
{
    [RawPrototype]
    public class DummyMsg : MonoBehaviour
    {
        private Text text;

        float startScale; float endScale; float time;

        float currTime;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        public void Initialize(int frontSize, float startScale, float endScale, float time, string msg, Color color)
        {
            text.fontSize = frontSize;
            text.color = color;
            text.text = msg;

            this.startScale = startScale;

            this.endScale = endScale;
            this.time = time;
        }

        private void Update()
        {
            currTime += Time.deltaTime;

            if (currTime >= time)
            {
                Destroy(gameObject);
            }
            else
            {
                var scale = Mathf.Lerp(startScale, endScale, currTime / time);
                transform.localScale = new Vector3(scale, scale, scale);
            }
        }


    }
}
