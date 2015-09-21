using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class Hex : AbstractHex
    {
        public static implicit operator int(Hex hex)
        {
            return hex.GetHashCode();
        }

        public int H { get; private set; }

        public List<Hex> Neighbors { get; protected set; }

        private int findFlag;
        public int FindFlag
        {
            get
            {
                return findFlag;
            }
            set
            {
                findFlag = value;
                if (findFlag != 0)
                {
                    GetComponentInChildren<TextMesh>().text = "";// findFlag.ToString();
                }
                else
                    GetComponentInChildren<TextMesh>().text = "";
            }
        }

        public IContainable Content { get; private set; }

        public Vector3 GroundCenter { get; private set; }



        protected override void Awake()
        {
            base.Awake();

            var hexEditor = GetComponent<HexEditor>();

            SetIndex(new Index2D(hexEditor.xIndex, hexEditor.yIndex));
            H = hexEditor.height;

            GroundCenter = new Vector3(t.position.x, t.position.y + GetComponent<Collider>().bounds.size.y, t.position.z);

            Destroy(hexEditor);

            t.GetChild(0).gameObject.SetActive(true);

            Content = EmptyContent.Get();

            GameMap.I.Add(this);
        }


        private void Start()
        {
            Neighbors = new List<Hex>();
            foreach (var index in NeighborsIndexes)
            {
                var hex = GameMap.I.Get(index);
                if (hex)
                {
                    Neighbors.Add(hex);
                }
            }
        }

        public void OnContentAppeared(IContainable content)
        {
            Content = content;
        }

        public void OnContentMissed(IContainable content)
        {
            if (Content == content)
                Content = EmptyContent.Get();
        }

        public bool HasFigure
        {
            get { return Content != null && Content.Type == ContentType.Figure; }
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
