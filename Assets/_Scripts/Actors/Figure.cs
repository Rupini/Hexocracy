using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class Figure : BouncingObject
    {
        protected Stat reds;
        protected Stat blues;
        protected Stat greens;

        protected DynamicStat hp;
        protected DynamicStat ap;
        protected Stat damage;
        protected Stat initiative;

        protected Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

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

        #region Initialize
        protected override void Awake()
        {
            base.Awake();
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteColor");
        }

        public override void Initialize(FigureContainer container, FigureData data)
        {
            base.Initialize(container, data);
            Owner = Player.GetByIndex(data.owner);

            r.material = Resources.Load<Material>("Models/Materials/whiteColor");
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteColor");
            r.material.color = data.color;

            InitStats(data);
        }

        protected virtual void InitStats(FigureData data)
        {
            Stat stat;

            reds = new Stat(StatType.Red, 0);
            stat = new DynamicStat(StatType.HealthPoints, data.baseHP, new List<Stat>() { reds }, x => x[0] * 2);
            hp = (DynamicStat)stat;
            stats.Add(stat.Type, stat);

            stat = new DynamicStat(StatType.ActionPoints, data.actionPoints);
            ap = (DynamicStat)stat;
            stats.Add(stat.Type, stat);

            greens = new Stat(StatType.Green, 0);
            stat = new Stat(StatType.Damage, data.damage, new List<Stat>() { greens }, x => x[0]);
            damage = stat;
            stats.Add(stat.Type, stat);

            blues = new Stat(StatType.Blue, 0);
            stat = new Stat(StatType.Initiative, data.initiative, new List<Stat>() { blues }, x => x[0] * 10f / (100 + x[0] * 10f));
            initiative = stat;
            stats.Add(stat.Type, stat);
        }

        #endregion
        #region Fighting

        public void OnAttack(Figure attacker, float dmg)
        {
            hp.CurrValue -= dmg;
            if (hp.CurrValue <= 0)
                Destroy();
        }

        #endregion
        #region Callback
        protected override void OnFigureCollided(Figure figure, int bounceHeight, bool forced)
        {
            if (figure.Owner.IsAlly(Owner))
            {
                if (forced && ap.CurrValue <= 0)
                    figure.OnAttack(this, (damage - bounceHeight * 2) * (-ap.CurrValue * 0.5f));

            }
            else
                figure.OnAttack(this, damage - bounceHeight * 2);
        }

        protected override void OnHexLanded(Hex hex, int bounceHeight)
        {
            if (bounceHeight < -1)
            {
                OnAttack(this, -bounceHeight * 2);
            }
        }

        protected override void OnTurnStarted(bool newRound)
        {
            if (newRound)
            {
                ap.CurrValue = ap.Value;
            }
        }

        protected override void OnTurnFinished()
        {
        }

        protected override void OnForcedMovePenalti()
        {
            OnAttack(this, -damage * ap.CurrValue);
        }
        #endregion
    }
}
