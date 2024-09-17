using System;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.States;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Bootstrap
{
    public class GameplaySceneBootstrapper : IInitializable
    {
        // [Inject] private GameplayStateMachine _gameplayStateMachine;
        [Inject] private BattleStateMachine _battleStateMachine;
        [Inject] private StateFactory _stateFactory;
        
        [Inject] private ConfigProvider _configProvider;
        
        public void Initialize()
        {
            _configProvider.LoadSingle<GameplaySceneConfig>(Constants.ConfigPath.MapFile + "Afgan");
            Debug.Log("Remove string literal");
            
            RegisterStates();
            SetBattleState();
        }

        private void RegisterStates()
        {
            // _gameplayStateMachine.RegisterState(_stateFactory.Create<PlayingState>());
            
            _battleStateMachine.RegisterState(_stateFactory.Create<StealthState>());
            _battleStateMachine.RegisterState(_stateFactory.Create<AlertState>());
        }
        
        private void SetBattleState()
        {
            var config = _configProvider.GetSingle<GameplaySceneConfig>();
            
            switch (config.InitialState)
            {
                case InitialState.Stealth:
                    _battleStateMachine.Enter<StealthState>();
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