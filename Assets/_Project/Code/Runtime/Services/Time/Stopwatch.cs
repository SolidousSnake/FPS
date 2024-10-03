using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.Services.Time
{
    public class Stopwatch
    {
        private CancellationTokenSource _cts = new();

        public event Action<TimeSpan> Ticked;
        public TimeSpan ElapsedTime { get; private set; }

        private async UniTask Tick()
        {
            var second = TimeSpan.FromSeconds(1f);
            while (!_cts.IsCancellationRequested)
            {
                ElapsedTime += second;
                Ticked?.Invoke(ElapsedTime);

                await UniTask.WaitForSeconds(1f, cancellationToken: _cts.Token);
            }
        }
        
        public void Start()
        {
            _cts = new CancellationTokenSource();
            Tick().Forget();
        }

        public void Stop() => _cts.Cancel();
        public void Reset() => ElapsedTime = TimeSpan.Zero;
    }
}