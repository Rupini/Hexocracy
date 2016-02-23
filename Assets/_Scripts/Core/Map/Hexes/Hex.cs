using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class Hex : AbstractHex //,IPFEdge
    {
        public static implicit operator int(Hex hex)
        {
            return hex.EntityID;
        }

        #region Definition

        private GameMap container;
        private int _findFlag;
        private List<IHexAddition> additions;

        public int H { get; private set; }

        public List<Hex> Neighbors { get; protected set; }

        public int FindFlag
        {
            get
            {
                return _findFlag;
            }
            set
            {
                _findFlag = value;
                if (_findFlag != 0)
                {
                    GetComponentInChildren<TextMesh>().text = "";// findFlag.ToString();
                }
                else
                    GetComponentInChildren<TextMesh>().text = "";
            }
        }

        public IContainable Content { get; private set; }

        public Vector3 GroundCenter { get; private set; }
        #endregion
        #region Initialize
        protected Hex()
        {
            additions = new List<IHexAddition>();
        }

        public void Initialize(GameMap container, HexData data)
        {
            this.container = container;

            SetIndex(new Index2D(data.xIndex, data.yIndex));
            H = data.height;
            t.localScale = new Vector3(t.localScale.x, data.scaleY, t.localScale.z);

            GroundCenter = new Vector3(t.position.x, t.position.y + GetComponent<Collider>().bounds.size.y, t.position.z);

            //*!Crutch
            t.GetChild(0).gameObject.SetActive(true);
            if (data.hasAddition)
            {
                t.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/ElementHex");
                AddAddition(new ItemSpawner(data.addition));
            }
            //_

            Content = EmptyContent.Get();
        }
        #endregion

        public override void DefineCircum()
        {
            var nihility = GameServices.Get<Nihility>();
            Neighbors = new List<Hex>();
            int i = 0;

            foreach (var index in CircumIndices)
            {
                var hex = container.Get(index);
                if (hex)
                {
                    Neighbors.Add(hex);
                    Circum[i] = hex;
                }
                else
                {
                    Circum[i] = nihility.GetDefinedNull(index);
                }

                i++;
            }
        }

        public void OnContentEntered(IContainable content)
        {
            Content = content;
        }

        public void OnContentLeft(IContainable content)
        {
            if (Content == content)
                Content = EmptyContent.Get();
        }

        public bool HasFigure
        {
            get { return Content.Type == ContentType.Figure; }
        }

        public bool IsNeighbor(Hex hex)
        {
            var dx = Mathf.Abs(Index.X - hex.Index.X);
            var dy = Mathf.Abs(Index.Y - hex.Index.Y);

            return dx != 2 && dx + dy == 2;
        }

        public void ChangeColor(Color color)
        {
            GetComponentInChildren<SpriteRenderer>().color = color;
        }

        public void PaintNeighbors(Color color)
        {
            Neighbors.ForEach((h) => { h.ChangeColor(color); });
        }

        private void AddAddition(IHexAddition addition)
        {
            additions.Add(addition);
            addition.Attach(this);
        }

        private void RemoveAddition(IHexAddition addition)
        {
            additions.Remove(addition);
            addition.Disattach();
        }

        public override string ToString()
        {
            return "Hex " + Index.ToString() + " " + FindFlag;
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }
    }
}
