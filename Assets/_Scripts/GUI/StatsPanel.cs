using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Hexocracy
{
    public class StatsPanel : MonoBehaviour
    {
        private Figure target;

        private Text text;

        private static StatsPanel instance;

        private void Awake()
        {
            instance = this;
            text = GetComponent<Text>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (target)
            {
                transform.position = Camera.main.WorldToScreenPoint(target.t.position + new Vector3(0, 2, 0));
                text.text = "<color=red>" + target.HP + "/" + target.MaxHP + "</color>\n" +
                    "<color=olive>" + target.AP + "/" + target.MaxAP + "</color>\n" +
                    "<color=aqua>" + Mathf.Round(target.Initiative)+ "</color>";
            }
            else
                gameObject.SetActive(false);
        }

        public static void SetTarget(Figure target)
        {
            instance.gameObject.SetActive(true);
            instance.target = target;
        }

    }
}
