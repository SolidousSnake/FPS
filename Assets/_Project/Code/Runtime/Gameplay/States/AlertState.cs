using System;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.Factory;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Services.Sound;
using _Project.Code.Runtime.Services.Time;
using _Project.Code.Runtime.UI.Label;
using _Project.Code.Runtime.UI.Parent;
using _Project.Code.Runtime.UI.View.State;
using UniRx;

namespace _Project.Code.Runtime.States
{
    public class AlertState : IState
    {
        private readonly MusicConfig _musicConfig;
        private readonly BattleStateMachine _fsm;
        private readonly MusicService _musicService;
        private readonly EnemyCollection _enemyCollection;

        private readonly Timer _timer;
        private readonly TimeLabel _timeLabel;
        private readonly StateView _stateView;

        private IDisposable _playerSighted;
        private IDisposable _ticked;
        private IDisposable _finished;

        public AlertState(BattleStateMachine fsm
            , MusicService musicService
            , ConfigProvider configProvider
            , EnemyCollection enemyCollection
            , UIFactory uiFactory
            , RadarParent radarParent
            , TimeLabel timeLabel)
        {
            _fsm = fsm;
            _musicService = musicService;
            _enemyCollection = enemyCollection;
            _timeLabel = timeLabel;

            var levelConfig = configProvider.GetSingle<LevelConfig>();
            _musicConfig = levelConfig.Music;

            _stateView = uiFactory.CreateView(radarParent.transform, InitialState.Alert);

            _timer = new Timer(TimeSpan.FromSeconds(levelConfig.AlertStateDuration));
            _stateView.Hide();
        }

        public void Enter()
        {
            _playerSighted = _enemyCollection.PlayerSighted.Subscribe(sighted =>
            {
                if (sighted) StopCountdown();
                else StartCountdown();
            });

            _ticked = _timer.ElapsedTime.Subscribe(_timeLabel.SetValue);

            _stateView.Show();
            _enemyCollection.SetChase();
            _musicService.PlayImmediately(_musicConfig.AlertLoopClip);
        }

        public void Exit()
        {
            _playerSighted.Dispose();
            _ticked.Dispose();
            _finished.Dispose();

            _stateView.Hide();
            _musicService.Stop();
        }

        private void StartCountdown()
        {
            _finished = _timer.ElapsedTime
                .Where(time => time <= TimeSpan.Zero)
                .Subscribe(_ => _fsm.Enter<EvasionState>());

            _timer.Start();
        }

        private void StopCountdown()
        {
            _timer.Stop();
            _timer.Reset();
        }
    }
}