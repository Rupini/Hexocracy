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
    public class Figure : BouncingObject, IActor, IAttacker
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

        private GameDependence<Stat> simpleAttack;
        private GameDependence<Stat> penaltiAllyAttack;
        private GameDependence<Stat> penaltyFalling;
        private GameDependence<Stat> penaltyForcedMove;

        #endregion

        #region Properties

        public Range RDamage { get; private set; }

        public float Damage { get { return RDamage; } }

        public override ContentType Type { get { return ContentType.Figure; } }

        #endregion

        #region Initialize
        protected Figure()
        {
            statHolder = new StatsHolder();
            EntityID = GetInstanceID();
        }

        protected override void Awake()
        {
            base.Awake();
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteColor");
        }

        [RawPrototype]
        public override void Initialize(FigureData data)
        {
            base.Initialize(data);
            Owner = Player.GetByIndex(data.owner);

            //*!Crutch
            r.material = Resources.Load<Material>("Models/Materials/whiteMat");
            r.material.mainTexture = Resources.Load<Texture>("Models/whiteMat");
            r.material.color = data.color;
            //_

            InitStats(data);
            InitFight();
        }


        #endregion

        #region Stats

        protected virtual void InitStats(FigureData data)
        {
            reds = statHolder.Add(StatType.Red, 0, false);
            greens = statHolder.Add(StatType.Green, 0, false);
            blues = statHolder.Add(StatType.Blue, 0, false);

            mass = statHolder.Add(StatType.Mass, 10, false);

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

        public float MaxAP { get { return ap; } }

        public override float AP
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

        [RawPrototype]
        public void AddElement(Element element)
        {
            statHolder[(StatType)element.Kind].BaseValue += element.Count;
        }

        #endregion

        #region Fighting

        protected virtual void InitFight()
        {
            var provider = new FigureDependenceProvider(this, statHolder.GetStats(), DependenceType.Damage);

            simpleAttack = provider.Get(DamageDepenenceType.SimpleAttack);
            penaltiAllyAttack = provider.Get(DamageDepenenceType.PenaltiAllyAttack);
            penaltyFalling = provider.Get(DamageDepenenceType.PenanltiFalling);
            penaltyForcedMove = provider.Get(DamageDepenenceType.PenaltiForcedMove);
        }

        public void OnAttack(IAttacker attacker, float dmg)
        {
            hp.CurrValue -= dmg;
            if (hp.CurrValue <= 0)
            {
                Destroy();
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
                    figure.OnAttack(this, penaltiAllyAttack.Calculate(-bounceHeight));
                }
            }
            else
            {
                figure.OnAttack(this, simpleAttack.Calculate(-bounceHeight));
            }
        }

        protected override void OnHexLanded(Hex hex, int bounceHeight)
        {
            if (bounceHeight < -jumpDownHeight)
            {
                OnAttack(this, penaltyFalling.Calculate(-bounceHeight));
            }
        }

        protected override void OnForcedMovePenalti()
        {
            OnAttack(this, penaltyForcedMove.Calculate());
        }

        protected override void OnContentContact(IContainable item)
        {
            ((ItemBox)item).Contact(this);
        }

        protected override void OnTurnStarted(bool isNewRound)
        {
            if (isNewRound)
            {
                AP = MaxAP;
            }
        }
        #endregion

        #region Implements

        #region IActivable

        public bool Active { get; protected set; }

        [RawPrototype]
        private Color defaultColor;

        [RawPrototype]
        public void Deactivate()
        {
            Active = false;
            Owner.SwitchActiveState(false);
            if (this)//Cruntch!
            {
                r.material.color = defaultColor;
            }
        }

        [RawPrototype]
        public void Activate()
        {
            Active = true;
            Owner.SwitchActiveState(true);
            defaultColor = r.material.color;
            r.material.color = new Color(1, 1, 1);
        }

        #endregion

        #endregion
    }
}
