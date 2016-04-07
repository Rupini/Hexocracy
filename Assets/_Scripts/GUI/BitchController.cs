using Hexocracy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Hexocracy.View
{
    [RawPrototype]
    public class BitchController : MonoBehaviour
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
            transform.parent = GameInstance.Canvas.transform;

            this.startScale = startScale;

            this.endScale = endScale;
            this.time = time;

            GetComponent<RectTransform>().localPosition = new Vector3();
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
