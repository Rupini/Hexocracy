using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class Figure : BouncingObject
    {
        public int BaseHP;

        private int _hp;

        protected Stat reds;
        protected Stat blues;
        protected Stat greens;

        protected DynamicStat hp;
        protected DynamicStat mp;
        protected DynamicStat ap;

        protected Stat damage;
        protected Stat armor;

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

        public int MaxMP { get { return mp; } }

        public override int MP
        {
            get
            {
                return (int)mp.CurrValue;
            }
            protected set
            {
                mp.CurrValue = value;
            }
        }

        protected Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

        #region Initialize
        protected override void Awake()
        {
            base.Awake();
            InitializeStats();
        }

        protected virtual void InitializeStats()
        {
            Stat stat;

            stat = new DynamicStat(StatType.HealthPoints, 40, 40);
            hp = (DynamicStat)stat;
            stats.Add(stat.Type, stat);

            stat = new DynamicStat(StatType.MovePoints, baseMovePoints, baseMovePoints);
            mp = (DynamicStat)stat;
            stats.Add(stat.Type, stat);

            stat = new Stat(StatType.Damage, 8);
            damage = stat;
            stats.Add(stat.Type, stat);

        }

        #endregion
        #region Fighting

        public void OnAttack(Figure attacker, float dmg)
        {
            hp.CurrValue -= 0;// dmg;
            if (hp.CurrValue <= 0)
                Destroy();
        }

        #endregion
        #region Callback
        protected override void OnFigureCollided(Figure figure, int bounceHeight, bool forced)
        {
            if (figure.Owner.IsAlly(Owner))
            {
                if (forced && mp.CurrValue <= 0)
                    figure.OnAttack(this, -damage * (mp.CurrValue * 0.5f));

            }
            else
                figure.OnAttack(this, damage);
        }

        protected override void OnHexLanded(Hex hex, int bounceHeight)
        {
            //if (bounceHeight < 0)
            //{
            //    OnAttack(this, -bounceHeight * damage);
            //}
        }

        protected override void OnRoundStarted(bool newCicle)
        {
            mp.CurrValue = mp.Value;
        }

        protected override void OnRoundFinished()
        {
        }

        protected override void OnForcedMovePenalti()
        {
            OnAttack(this, -damage * mp.CurrValue);
        }
        #endregion
    }
}
