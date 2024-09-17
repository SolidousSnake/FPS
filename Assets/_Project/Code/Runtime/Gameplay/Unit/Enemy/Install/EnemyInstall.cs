using System;
using System.Collections.Generic;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.DamageCalculator;
using _Project.Code.Runtime.States;
using _Project.Code.Runtime.UI.Bar;
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

        [Header("Behaviour")] [SerializeField] private bool _patrol;

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

        [Inject] private BattleStateMachine _battleStateMachine;
        [Inject] private ConfigProvider _configProvider;

        private IDamageCalculator _calculator;
        private StateMachine _enemyStateMachine;
        private EnemySpeaker _enemySpeaker;

        private void Start()
        {
            Debug.Log("Initialized from Start");
            Initialize();
        }

        public virtual void Initialize()
        {
            _enemyStateMachine = new StateMachine();
            _enemySpeaker = new EnemySpeaker(_speechSource, _configProvider, gameObject.name);

            InstallStates();
            InstallDamageCalculator();

            if (ShouldHaveArmor())
                _armorBar.Initialize(_armor);

            foreach (var hitBox in _hitBoxes)
                hitBox.Initialize(_calculator);

            _healthBar.Initialize(_health);
            Subscribe();
        }
    
        private void Subscribe()
        {
            _visionSensor.TargetSighted += _battleStateMachine.Enter<AlertState>;
            _visionSensor.TargetLost += _battleStateMachine.Enter<StealthState>;
        }

        private void OnDestroy()
        {
            _visionSensor.TargetSighted -= _battleStateMachine.Enter<AlertState>;
            _visionSensor.TargetLost -= _battleStateMachine.Enter<StealthState>;
        }

        protected virtual void InstallStates()
        {
            _enemyStateMachine.RegisterState(new ChaseTarget(_enemySpeaker));
            _enemyStateMachine.RegisterState(new SearchState(_enemySpeaker));
            _enemyStateMachine.RegisterState(new DeathState(_enemySpeaker));
            _enemyStateMachine.RegisterState(new AttackState(_enemyStateMachine, _visionSensor, _enemySpeaker, _aimLayer));

            if (_patrol)
            {
                _enemyStateMachine.RegisterState(new PatrolState(_enemyStateMachine, _visionSensor,
                    new WaypointMovement(_waypoints, _movementMode), new NavMeshMovement(_navMeshAgent)));
                _enemyStateMachine.Enter<PatrolState>();
            }
            else
            {
                _enemyStateMachine.RegisterState(new SentryState(_enemyStateMachine, _visionSensor));
                _enemyStateMachine.Enter<SentryState>();
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

        [Button]
        private void GetHitBoxes() => _hitBoxes = GetComponentsInChildren<UnitHitBox>();

        private bool ShouldHaveArmor() => _damageCalculatorType != DamageCalculatorType.OnlyArmor;
    }
}