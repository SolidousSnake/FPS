using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace _Project.Code.Runtime.Services.Time
{
    public class Timer
    {
        private readonly TimeSpan _initialTime;
        private CancellationTokenSource _cts;
        private ReactiveProperty<TimeSpan> _elapsedTime;
        
        public Timer(TimeSpan initialTime)
        {
            _initialTime = initialTime;
            _elapsedTime = new ReactiveProperty<TimeSpan>(_initialTime);
        }

        public IReactiveProperty<TimeSpan> ElapsedTime => _elapsedTime; 

        private async UniTask Tick()
        {
            var millisecond = TimeSpan.FromMilliseconds(10);
            while (!_cts.IsCancellationRequested)
            {
                _elapsedTime.Value -= millisecond;

                if (_elapsedTime.Value <= TimeSpan.Zero)
                {
                    _elapsedTime.Value = TimeSpan.Zero;
                    Stop();                      
                }
                
                await UniTask.Delay(millisecond, cancellationToken: _cts.Token); 
            }
        }
        
        public void Start()
        {
            _cts = new CancellationTokenSource();
            Reset();
            Tick().Forget();
        }

        public void Stop() => _cts?.Cancel();
        public void Reset() => _elapsedTime.Value = _initialTime;
    }
}