using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    
    public class Hex : AbstractHex //,IPFEdge
    {
        #region Static
        public static implicit operator int(Hex hex)
        {
            return hex.EntityID;
        }
        #endregion

        #region Definition

        private GameMap map;
        private Nihility nihility;
        private List<IHexAddition> additions;

        private int _findFlag;

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
            }
        }

        public IContainable Content { get; private set; }

        public Vector3 GroundCenter { get; private set; }
        #endregion

        #region Initialize

        protected override void Awake()
        {
            base.Awake();

            additions = new List<IHexAddition>();
            map = GS.Get<GameMap>();
            nihility = GS.Get<Nihility>();
        }

        public void Initialize(HexData data)
        {
            SetIndex(new Index2D(data.xIndex, data.yIndex));
            H = data.height;
            t.localScale = new Vector3(t.localScale.x, data.scaleY, t.localScale.z);

            GroundCenter = new Vector3(t.position.x, t.position.y + r.bounds.size.y * 0.5f, t.position.z);

            if (data.hasAddition)
            {
                AddAddition(new ElementSpawner(data.addition));
            }

            Content = EmptyContent.Get();
        }
        #endregion

        protected override void OnTurnStarted(bool isNewRound)
        {
            foreach(var addition in additions)
            {
                addition.OnTurnUpdate(isNewRound);
            }
        }

        public override void DefineCircum()
        {
            Circum = new IHex[6];
            Neighbors = new List<Hex>();
            int i = 0;

            foreach (var index in CircumIndices)
            {
                var hex = map.Get(index);
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
            {
                Content = EmptyContent.Get();
            }
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
            return Index;
        }
    }
}
