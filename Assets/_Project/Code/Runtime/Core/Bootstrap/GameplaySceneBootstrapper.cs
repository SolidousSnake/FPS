using System;
using System.Collections.Generic;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Point.SpawnPoint;
using _Project.Code.Runtime.States;
using _Project.Code.Runtime.UI.Label;
using Zenject;

namespace _Project.Code.Runtime.Core.Bootstrap
{
    public class GameplaySceneBootstrapper : IInitializable
    {
        [Inject] private readonly BattleStateMachine _battleStateMachine;
        [Inject] private readonly StateFactory _stateFactory;
        [Inject] private readonly ConfigProvider _configProvider;

        [Inject] private readonly TimeLabel _timeLabel;
        
        [Inject] private readonly IEnumerable<EnemySpawnPoint> _spawnPoints;
        
        public void Initialize()
        {
            _configProvider.LoadSingle<LevelConfig>(AssetPath.ConfigPath.MapFile + "Afgan");
            RegisterStates();
            SetBattleState();

            _timeLabel.Initialize();
            
            foreach (var spawnPoint in _spawnPoints) 
                spawnPoint.Spawn();
        }

        private void RegisterStates()
        {
            _battleStateMachine.RegisterState(_stateFactory.Create<StealthState>());
            _battleStateMachine.RegisterState(_stateFactory.Create<AlertState>());
            _battleStateMachine.RegisterState(_stateFactory.Create<EvasionState>());
        }
        
        private void SetBattleState()
        {
            var config = _configProvider.GetSingle<LevelConfig>();
            
            switch (config.InitialState)
            {
                case InitialState.Stealth:
                    _battleStateMachine.Enter<StealthState>();
                    break;
                case InitialState.Evasion:
                    _battleStateMachine.Enter<EvasionState>();
                    break;
                case InitialState.Alert:
                    _battleStateMachine.Enter<AlertState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Put {config.InitialState} in the gameplay bootstrapper");
            }
        }
    }
}