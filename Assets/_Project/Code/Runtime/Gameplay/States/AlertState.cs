using System.Threading;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Services.Sound;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.States
{
    public class AlertState : IState
    {
        private readonly MusicConfig _musicConfig;
        private readonly BattleStateMachine _fsm;
        private readonly MusicService _musicService;
        private readonly EnemyCollection _enemyCollection;
        private readonly float _stateDuration;

        private CancellationTokenSource _cts;

        public AlertState(BattleStateMachine fsm, MusicService musicService, ConfigProvider configProvider,
            EnemyCollection enemyCollection)
        {
            _fsm = fsm;
            _musicService = musicService;
            _enemyCollection = enemyCollection;

            var levelConfig = configProvider.GetSingle<LevelConfig>();
            _musicConfig = levelConfig.Music;
            _stateDuration = levelConfig.AlertStateDuration;
        }

        public void Enter()
        {
            _enemyCollection.PlayerSighted += StopCountdown;
            _enemyCollection.PlayerLost += StartCountdown;

            _enemyCollection.SetChase();
            _musicService.PlayImmediately(_musicConfig.AlertLoopClip);
        }

        public void Exit()
        {
            _enemyCollection.PlayerSighted -= StopCountdown;
            _enemyCollection.PlayerLost -= StartCountdown;

            _musicService.Stop();
        }

        private void StopCountdown() => _cts?.Cancel();

        private async void StartCountdown()
        {
            _cts = new CancellationTokenSource();
            await UniTask.WaitForSeconds(_stateDuration, cancellationToken: _cts.Token);
            _fsm.Enter<EvasionState>();
        }
    }
}