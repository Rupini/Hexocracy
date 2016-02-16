using Hexocracy.HelpTools;
using Hexocracy.Mech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Hexocracy.Core
{
    public class Figure : BouncingObject, IActivableObject, IAttacker
    {
        #region Definition
        protected StatsHolder statHolder;
        
        protected Stat reds;
        protected Stat blues;
        protected Stat greens;
        protected Stat mass;

        protected DynamicStat hp;
        protected DynamicStat ap;
        protected Stat minDamage;
        protected Stat maxDamage;
        protected Stat initiative;

        public Range RDamage { get; private set; }

        public float Damage { get { return RDamage; } }

        #endregion
        #region Properties

        public override ContentType Type { get { return ContentType.Figure; } }

        #endregion
        #region Initialize
        protected Figure()
        {
            statHolder = new StatsHolder();
        }

        protected override void Awake()
        {
            base.Awake();
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteColor");
        }

        public override void Initialize(FigureContainer container, FigureData data)
        {
            base.Initialize(container, data);
            Owner = Player.GetByIndex(data.owner);

            //*!Crutch
            r.material = Resources.Load<Material>("Models/Materials/whiteMat");
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteMat");
            r.material.color = data.color;
            //_

            InitStats(data);
        }

       
        #endregion
        #region Stats

        protected virtual void InitStats(FigureData data)
        {
            reds = statHolder.Add(StatType.Red, 0, false);
            greens = statHolder.Add(StatType.Green, 0, false);
            blues = statHolder.Add(StatType.Blue, 0, false);

            mass = statHolder.Add(StatType.Mass, 0, false);

            hp = (DynamicStat)statHolder.Add(StatType.HealthPoints, data.baseHP, true);
            ap = (DynamicStat)statHolder.Add(StatType.ActionPoints, data.actionPoints, true); ;

            maxDamage = statHolder.Add(StatType.MaxDamage, data.damage, false);
            minDamage = statHolder.Add(StatType.MinDamage, 0, false);

            initiative = statHolder.Add(StatType.Initiative, data.initiative, false);

            statHolder.InitializeDependencies();

            RDamage = new Range(() => minDamage.Value, () => maxDamage);
        }

        public float MaxHP { get { return hp; } }

        public float HP
        {
            get
            {
                return hp.CurrValue;
            }
            protected set
            {
                hp.CurrValue = value;
            }
        }

        public int MaxAP { get { return ap; } }

        public override int AP
        {
            get
            {
                return (int)ap.CurrValue;
            }
            protected set
            {
                ap.CurrValue = value;
            }
        }

        public float Initiative { get { return initiative; } }

        public void AddElement(Element element)
        {
            statHolder[(StatType)element.Kind].BaseValue += element.Count;
        }

        #endregion
        #region Fighting

        public void OnAttack(IAttacker attacker, float dmg)
        {
            hp.CurrValue -= dmg;
            if (hp.CurrValue <= 0)
                Destroy();
        }

        private int bombCD = 3;
        public int bombCurrCD = 0;

        public void CreateBomb()
        {
            if (bombCurrCD == 0)
            {
                bombCurrCD = bombCD;

                var bomb = new BombData();
                bomb.damage = URandom.Range(3f, 4f) * mass;
                bomb.type = ItemType.Other;
                bomb.lifeTime = 5;
                bomb.yOffsetK = 0;

                ItemFactory.I.Create(bomb, currentHex, Owner);
            }
        }

        #endregion
        #region Callback
        protected override void OnFigureCollided(Figure figure, int bounceHeight, bool forced)
        {
            if (figure.Owner.IsAlly(Owner))
            {
                if (forced && ap.CurrValue <= 0)
                {
                    figure.OnAttack(this, (Damage - bounceHeight * 2) * (-ap.CurrValue * 0.5f));
                }
            }
            else
                figure.OnAttack(this, Damage - bounceHeight * 2);
        }

        protected override void OnHexLanded(Hex hex, int bounceHeight)
        {
            if (bounceHeight < -1)
            {
                OnAttack(this, -bounceHeight * 2);
            }
        }

        protected override void OnForcedMovePenalti()
        {
            OnAttack(this, -Damage * ap.CurrValue);
        }

        protected override void OnTurnStarted(bool newRound)
        {
            if (newRound)
            {
                ap.CurrValue = ap.Value;
                if (bombCurrCD != 0)
                {
                    bombCurrCD--;
                }
            }
        }

        protected override void OnItemBoxContact(IContainable item)
        {
            ((ItemBox)item).Contact(this);
        }
        #endregion

        #region Implements

        #region IActivable

        public bool Active { get; protected set; }
        private Color defaultColor;

        public void Deactivate()
        {
            Active = false;
            Owner.SwitchActiveState(false);
            if (this)//Cruntch!
            {
                r.material.color = defaultColor;
            }
        }

        public void Activate()
        {
            Active = true;
            Owner.SwitchActiveState(true);
            defaultColor = r.material.color;
            r.material.color = new Color(1, 1, 1);
        }

        public int EntityID { get { return GetInstanceID(); } }

        #endregion

        #endregion
    }
}
