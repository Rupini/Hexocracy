using Hexocracy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Hexocracy.HUD
{
    [RawPrototype]
    public class StatsPanel : MonoBehaviour
    {
        private Figure target;

        private Text text;

        private void Awake()
        {
            text = gameObject.GetComponent<Text>();
        }

        private void Update()
        {
            if (target)
            {
                transform.position = Camera.main.WorldToScreenPoint(target.t.position + new Vector3(0, 2, 0));
                text.text = "<color=red>" + (int)target.HP + "/" + (int)target.MaxHP + "</color><color=brown>/" + (int)target.Satiety + "</color>\n" +
                            "<color=green>" + target.RDamage + "</color>\n" +
                            "<color=blue>" + target.AP + "/" + target.MaxAP + "</color>";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetTarget(Figure target)
        {
            this.target = target;
        }

    }
}
