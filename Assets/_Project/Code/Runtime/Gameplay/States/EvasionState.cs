using System;
using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.Factory;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Services.Sound;
using _Project.Code.Runtime.Services.Time;
using _Project.Code.Runtime.UI.Label;
using _Project.Code.Runtime.UI.Parent;
using _Project.Code.Runtime.UI.View.State;
using UnityEngine;
using UniRx;

namespace _Project.Code.Runtime.States
{
    public class EvasionState : IState
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

        public EvasionState(BattleStateMachine fsm
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

            _stateView = uiFactory.CreateView(radarParent.transform, InitialState.Evasion);

            _timer = new Timer(TimeSpan.FromSeconds(levelConfig.AlertStateDuration));
            _stateView.Hide();
        }

        public void Enter()
        {
            _playerSighted = _enemyCollection.PlayerSighted.Subscribe(sighted =>
            {
                if (sighted) StopCountdown();
            });
            
            _ticked = _timer.ElapsedTime.Subscribe(_timeLabel.SetValue);
            _finished = _timer.ElapsedTime
                .Where(time => time <= TimeSpan.Zero)
                .Subscribe(_ => EnterStealthState());
            
            _stateView.Show();
            _enemyCollection.SetSearch();
            _musicService.PlayImmediately(_musicConfig.EvasionLoopClip);

            StartCountdown();
        }

        public void Exit()
        {
            _stateView.Hide();
            _musicService.Stop();
            _timer.Stop();
            _playerSighted.Dispose();
            _finished.Dispose();
            _ticked.Dispose();
        }

        private void StopCountdown()
        {
            _fsm.Enter<AlertState>();
        }

        private void StartCountdown() => _timer.Start();

        private void EnterStealthState()
        {
            Debug.Log("Evasion countdown finished, entering StealthState.");
            _fsm.Enter<StealthState>();
        }
    }
}