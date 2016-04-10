using Hexocracy.HelpTools;
using Hexocracy.Mech;
using Hexocracy.Systems;
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

        protected Stat satiety;
        protected Stat hunger;

        private Dictionary<FigureDependenceType, GameDependence<Stat>> dependencies;

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
            r.material.mainTexture = RM.LoadTexture("whiteColor");
        }

        [RawPrototype]
        public override void Initialize(FigureData data)
        {
            base.Initialize(data);

            Owner = Player.GetByIndex(data.playerIndex);

            //*!Crutch
            r.material = RM.LoadMaterial("whiteMat");
            r.material.mainTexture = RM.LoadTexture("whiteMat");
            r.material.color = Owner.TeamColor;
            //_

            InitStats(data);
            InitFight();
        }


        #endregion

        #region Stats

        protected virtual void InitStats(FigureData data)
        {
            reds = statHolder.Add(StatType.Red, false);
            greens = statHolder.Add(StatType.Green, false);
            blues = statHolder.Add(StatType.Blue, false);

            mass = statHolder.Add(StatType.Mass, false, data.baseMass);

            hp = (DynamicStat)statHolder.Add(StatType.HealthPoints, true, data.baseHP);
            ap = (DynamicStat)statHolder.Add(StatType.ActionPoints, true, data.actionPoints);

            maxDamage = statHolder.Add(StatType.MaxDamage, false, data.damage);
            minDamage = statHolder.Add(StatType.MinDamage, false);

            satiety = statHolder.Add(StatType.Satiety, false, data.satiety);
            hunger = statHolder.Add(StatType.Hunger, false);

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
                if (hp.CurrValue <= 0)
                {
                    Destroy();
                }
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

        public float Satiety
        {
            get
            {
                return satiety;
            }
            protected set
            {
                satiety.BaseValue = value;
            }
        }

        public void AddElement(Element element)
        {
            statHolder[(StatType)element.Kind].BaseValue += element.Count;
            Satiety = dependencies[FigureDependenceType.SaturationByElement].Calculate(element.Count);
        }


        #endregion

        #region Fighting

        protected virtual void InitFight()
        {
            var provider = new FigureDependenceProvider(this, statHolder.GetStats(), DependenceType.Damage);

            dependencies = new Dictionary<FigureDependenceType, GameDependence<Stat>>();

            foreach (FigureDependenceType depType in Enum.GetValues(typeof(FigureDependenceType)))
            {
                dependencies[depType] = provider.Get(depType);
            }


        }

        public void OnAttack(IAttacker attacker, float dmg)
        {
            HP -= dmg;
        }

        #endregion

        #region Callback
        protected override void OnFigureCollided(Figure figure, int bounceHeight, bool forced)
        {
            if (figure.Owner.IsAlly(Owner))
            {
                if (forced && ap.CurrValue <= 0)
                {
                    figure.OnAttack(this, dependencies[FigureDependenceType.PenaltiAllyAttack].Calculate(-bounceHeight));
                }
            }
            else
            {
                figure.OnAttack(this, dependencies[FigureDependenceType.SimpleAttack].Calculate(-bounceHeight));
            }
        }

        protected override void OnHexLanded(Hex hex, int bounceHeight)
        {
            if (bounceHeight < -jumpDownHeight)
            {
                OnAttack(this, dependencies[FigureDependenceType.PenanltiFalling].Calculate(-bounceHeight));
            }
        }

        protected override void OnForcedMovePenalti()
        {
            OnAttack(this, dependencies[FigureDependenceType.PenaltiForcedMove].Calculate());
        }

        protected override void OnContentContact(IContainable item)
        {
            ((ItemBox)item).Contact(this);
        }

        protected override void OnTurnStarted(bool isNewRound)
        {
            if (isNewRound)
            {
                AP = dependencies[FigureDependenceType.APRegenByTurn].Calculate();
                Satiety = dependencies[FigureDependenceType.DeptionByTurn].Calculate();
            }
        }

        protected override void OnTurnFinished(bool roundFinished)
        {
            if (roundFinished)
            {
                HP = dependencies[FigureDependenceType.HPLoseByHunger].Calculate();
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
