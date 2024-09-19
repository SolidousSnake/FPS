using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.States;
using _Project.Code.Runtime.Unit.Enemy.Install;
using _Project.Code.Runtime.Unit.Speaker;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class DeathState : IState
    {
        private readonly StateMachine _battleFsm;
        private readonly EnemySpeaker _speaker;
        private readonly EnemyInstall _selfObject;

        public DeathState(StateMachine battleFsm, EnemySpeaker speaker, EnemyInstall selfObject)
        {
            _battleFsm = battleFsm;
            _speaker = speaker;
            _selfObject = selfObject;
        }

        public async void Enter()
        {
            _speaker.SpeakDeath();
            _battleFsm.Enter<StealthState>();
            await UniTask.WaitForSeconds(5f);
            Destroy();
        }

        public void Exit()
        {
        }

        private void Destroy()
        {
            Object.Destroy(_selfObject.gameObject);
        }
    }
}