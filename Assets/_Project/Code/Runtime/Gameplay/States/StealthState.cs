using System;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Services.Sound;
using UniRx;

namespace _Project.Code.Runtime.States
{
    public class StealthState : IState
    {
        private readonly MusicConfig _musicConfig;
        private readonly BattleStateMachine _fsm;
        private readonly MusicService _musicService;
        private readonly EnemyCollection _enemyCollection;

        private IDisposable _playerSighted;

        public StealthState(BattleStateMachine fsm
            , MusicService musicService
            , ConfigProvider configProvider
            , EnemyCollection enemyCollection)
        {
            _fsm = fsm;
            _musicService = musicService;
            _enemyCollection = enemyCollection;
            _musicConfig = configProvider.GetSingle<LevelConfig>().Music;
        }

        public void Enter()
        {
            _playerSighted = _enemyCollection.PlayerSighted.Subscribe(sighted =>
            {
                if (sighted) _fsm.Enter<AlertState>();
            });

            _enemyCollection.SetIdle();
            _musicService.PlayImmediately(_musicConfig.StealthLoopClip);
        }

        public void Exit()
        {
            _playerSighted.Dispose();
            _musicService.StopAndReset();
        }
    }
}