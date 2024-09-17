using System;
using System.Threading;
using _Project.Code.Runtime.Config.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Code.Runtime.Weapon
{
    public sealed class Spread
    {
        private readonly AccuracyConfig _config;

        private float _minSpreadFactor;
        private float _maxSpreadFactor;
        private float _currentSpreadFactor;

        private CancellationTokenSource _cts;

        public Spread(AccuracyConfig config)
        {
            _config = config;
        }

        public event Action<float, float> Changed;

        public void ChangeSpreadFactor(bool aimed)
        {
            if (aimed)
            {
                _maxSpreadFactor = _config.MaxAimSpreadFactor;
                _minSpreadFactor = _config.MinAimSpreadFactor;
            }
            else
            {
                _maxSpreadFactor = _config.MaxHipSpreadFactor;
                _minSpreadFactor = _config.MinHipSpreadFactor;
            }

            _currentSpreadFactor = _minSpreadFactor;
            Changed?.Invoke(_currentSpreadFactor, 0f);
        }
        
        public Vector3 Get() => Random.insideUnitCircle * (_currentSpreadFactor) / 2f;
        
        public void IncreaseSpread()
        {
            _currentSpreadFactor = Mathf.Min(_currentSpreadFactor + _config.SpreadStep, _maxSpreadFactor);
            Changed?.Invoke(_currentSpreadFactor, _config.SpreadStep);
        }

        private void ResetSpread()
        {
            _currentSpreadFactor = _minSpreadFactor;
            Changed?.Invoke(_currentSpreadFactor, _config.SpreadRecoverySpeed);
        }

        public void StartResetSpreadTimer()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            ResetSpreadAfterTime(_config.SpreadRecoverySpeed, _cts.Token).Forget();
        }

        private async UniTaskVoid ResetSpreadAfterTime(float time, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
            ResetSpread();
        }
    }
}