using System;
using System.Collections.Generic;
using _Project.Code.Runtime.Config.AI.Quotes;
using _Project.Code.Runtime.Config.AI.Stats;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.DamageCalculator;
using _Project.Code.Runtime.UI.Bar;
using _Project.Code.Runtime.Unit.AI.Action;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.AI.Waypoint;
using _Project.Code.Runtime.Unit.Enemy.States;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.Speaker;
using _Project.Code.Runtime.Weapon.Visitor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Zenject;

namespace _Project.Code.Runtime.Unit.Enemy.Install
{
    public class EnemyInstall : MonoBehaviour
    {
        [SerializeField] private UnitHitBox[] _hitBoxes;
        [SerializeField] private DamageCalculatorType _damageCalculatorType;
        [SerializeField] private VisionSensor _visionSensor;

        [Title("Behaviour")] [SerializeField] private bool _patrol;

        [ShowIf(nameof(_patrol))] [TabGroup("Patrol")] [SerializeField]
        private MovementMode _movementMode;

        [ShowIf(nameof(_patrol))] [TabGroup("Patrol")] [SerializeField]
        private List<Waypoint> _waypoints;

        [TabGroup("Component")] [SerializeField]
        private AudioSource _speechSource;

        [TabGroup("Component")] [SerializeField]
        private NavMeshAgent _navMeshAgent;

        [TabGroup("Component")] [SerializeField]
        private Rig _aimLayer;

        [TabGroup("Common")] [SerializeField] private EnemyHealthBar _healthBar;
        [TabGroup("Common")] [SerializeField] private Health.Health _health;

        [TabGroup("Common")] [ShowIf(nameof(ShouldHaveArmor))] [SerializeField]
        private EnemyArmorBar _armorBar;

        [TabGroup("Common")] [ShowIf(nameof(ShouldHaveArmor))] [SerializeField]
        private Armor.Armor _armor;

        [Title("Debug")] [SerializeField] private float _maxAttackDistanceDebug;
        
        [Inject] private BattleStateMachine _battleFsm;
        [Inject] private ConfigProvider _configProvider;

        private IDamageCalculator _calculator;
        private StateMachine _enemyFsm;
        private EnemySpeaker _enemySpeaker;
        private AiStatsConfig _aiStatsConfig;
        
        private void Start()
        {
            Debug.Log("Initialized from Start");
            Initialize();
        }

        private void Update()
        {
            Debug.Log(_enemyFsm.GetState());
        }

        public virtual void Initialize()
        {
            _enemyFsm = new StateMachine();
            _enemySpeaker = new EnemySpeaker(_speechSource
                , _configProvider.GetSingleImmediately<QuotesConfig>(Constants.ConfigPath.AiQuotes + gameObject.name));

            InstallStates();
            InstallDamageCalculator();

            if (ShouldHaveArmor())
                _armorBar.Initialize(_armor);

            foreach (var hitBox in _hitBoxes)
                hitBox.Initialize(_calculator);

            _healthBar.Initialize(_health);
        }

        protected virtual void InstallStates()
        {
            var navMeshMovement = new NavMeshMovement(_navMeshAgent);
            var waypointMovement = new WaypointMovement(_waypoints, _movementMode);
            var findTargetAction = new FindClosestTargetAction(_visionSensor);
            
            _aiStatsConfig =
                _configProvider.GetSingleImmediately<AiStatsConfig>(Constants.ConfigPath.AiStats + gameObject.name);

            _enemyFsm.RegisterState(new ChaseTargetState(_enemyFsm, _enemySpeaker, _visionSensor, findTargetAction, _aiStatsConfig, navMeshMovement));
            _enemyFsm.RegisterState(new SearchState(_enemyFsm, _visionSensor, _enemySpeaker));
            _enemyFsm.RegisterState(new AttackState(_enemyFsm, _visionSensor, _enemySpeaker, findTargetAction, _aiStatsConfig, _aimLayer));
            _enemyFsm.RegisterState(new DeathState(_enemySpeaker));

            if (_patrol)
            {
                _enemyFsm.RegisterState(new PatrolState(_enemyFsm, _visionSensor, waypointMovement, navMeshMovement, _aimLayer));
                _enemyFsm.Enter<PatrolState>();
            }
            else
            {
                _enemyFsm.RegisterState(new SentryState(_enemyFsm, _visionSensor, _aimLayer));
                _enemyFsm.Enter<SentryState>();
            }
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