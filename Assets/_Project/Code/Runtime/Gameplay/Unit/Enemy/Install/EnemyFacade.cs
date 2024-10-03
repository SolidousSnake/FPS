using System;
using System.Collections.Generic;
using _Project.Code.Runtime.AI.States;
using _Project.Code.Runtime.Config.AI.Quotes;
using _Project.Code.Runtime.Config.AI.Stats;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.Factory;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.DamageCalculator;
using _Project.Code.Runtime.Point.Waypoint;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.UI.Parent;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.Player;
using _Project.Code.Runtime.Unit.Speaker;
using _Project.Code.Runtime.Weapon.Visitor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Zenject;

namespace _Project.Code.Runtime.Unit.Enemy.Install
{
    public class EnemyFacade : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private DamageCalculatorType _damageCalculatorType;

        [TabGroup("Component")] [SerializeField]
        private AudioSource _speechSource;

        [TabGroup("Component")] [SerializeField]
        private NavMeshAgent _navMeshAgent;

        [TabGroup("Component")] [SerializeField]
        private Rig _aimLayer;

        [TabGroup("Component")] [SerializeField]
        private UnitHitBox[] _hitBoxes;

        [TabGroup("Component")] [SerializeField]
        private VisionSensor _visionSensor;

        [TabGroup("Common")] [SerializeField] private Health.Health _health;

        [TabGroup("Common")] [ShowIf(nameof(ShouldHaveArmor))] [SerializeField]
        private Armor.Armor _armor;

        [Title("Debug")] [SerializeField] private float _maxAttackDistanceDebug;

        [Inject] private readonly ConfigProvider _configProvider;
        [Inject] private readonly UIFactory _uiFactory;
        [Inject] private readonly BossUiParent _bossUiParent;
        [Inject] private readonly EnemyCollection _enemyCollection;
        [Inject] private readonly PlayerFacade _player;

        private List<Waypoint> _waypoints;
        private MovementMode _movementMode;

        private IDamageCalculator _calculator;
        private EnemySpeaker _speaker;
        private AiStatsConfig _aiStatsConfig;
        public StateMachine Fsm { get; private set; }
        public bool CanSeePlayer => _visionSensor.CanSeePlayer;

        public string State;

        private void Update() => State = Fsm.ActiveState.GetType().Name;

        public void Construct(List<Waypoint> waypoints, MovementMode movementMode)
        {
            _waypoints = waypoints;
            _movementMode = movementMode;
            Construct();
        }

        public void Construct()
        {
            Fsm = new StateMachine();
            _speaker = new EnemySpeaker(_speechSource
                , _configProvider.GetSingleImmediately<QuotesConfig>(AssetPath.ConfigPath.AiQuotes + _name));

            InstallStates();
            InstallDamageCalculator();
            InitializeUI();

            foreach (var hitBox in _hitBoxes)
                hitBox.Initialize(_calculator);

            _enemyCollection.Register(this);
            _health.Depleted += Fsm.Enter<DeathState>;
        }

        private void OnDestroy()
        {
            _health.Depleted -= Fsm.Enter<DeathState>;
        }

        private void InitializeUI()
        {
            var bossNameLabel = _uiFactory.CreateBossNameLabel(_bossUiParent.transform);
            var healthBar = _uiFactory.CreateHealthBar(bossNameLabel.transform);
            var armorBar = _uiFactory.CreateArmorBar(bossNameLabel.transform);

            bossNameLabel.Initialize("SED");
            healthBar.Initialize(_health);

            if (ShouldHaveArmor())
                armorBar.Initialize(_armor);
        }

        private void InstallStates()
        {
            var navMeshMovement = new NavMeshMovement(_navMeshAgent);
            var waypointMovement = new WaypointMovement(_waypoints, _movementMode);

            _aiStatsConfig = _configProvider.GetSingleImmediately<AiStatsConfig>(AssetPath.ConfigPath.AiStats + _name);

            Fsm.RegisterState(new AttackState(_aiStatsConfig, Fsm, _visionSensor, _speaker, _aimLayer, _player));
            Fsm.RegisterState(new ChaseTargetState(_enemyCollection, _aiStatsConfig, Fsm, _visionSensor, _speaker, navMeshMovement,
                _player));
            Fsm.RegisterState(new SearchState(_enemyCollection, _aiStatsConfig, Fsm, _visionSensor, _speaker, navMeshMovement, _player));
            Fsm.RegisterState(new DeathState(_enemyCollection, this, _speaker, _aimLayer));

            if (_movementMode == MovementMode.None)
                Fsm.RegisterState(new SentryState(_enemyCollection, Fsm, _visionSensor, _aimLayer));
            else
                Fsm.RegisterState(new PatrolState(Fsm, _visionSensor, _aimLayer, waypointMovement, navMeshMovement));

            Fsm.EnterDefaultState();
        }

        private void InstallDamageCalculator()
        {
            _calculator = _damageCalculatorType switch
            {
                DamageCalculatorType.FirstArmorLastHealth => new FirstArmorDamageCalculator(_health, _armor),
                DamageCalculatorType.Ratio => new RatioDamageCalculator(_health, _armor),
                DamageCalculatorType.OnlyHealth => new OnlyHealthDamageCalculator(_health),
                DamageCalculatorType.OnlyArmor => new OnlyArmorDamageCalculator(_armor),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _maxAttackDistanceDebug);
        }

        [Button]
        private void GetHitBoxes() => _hitBoxes = GetComponentsInChildren<UnitHitBox>();

        private bool ShouldHaveArmor() => _damageCalculatorType != DamageCalculatorType.OnlyArmor;
    }
}