using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.States;
using _Project.Code.Runtime.Unit.Enemy.Install;
using _Project.Code.Runtime.Unit.Speaker;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class DeathState : IState
    {
        private readonly StateMachine _battleFsm;
        private readonly EnemySpeaker _speaker;
        private readonly Rig _aimLayer;
        private readonly EnemyInstall _selfObject;

        public DeathState(StateMachine battleFsm, EnemySpeaker speaker, Rig aimLayer, EnemyInstall selfObject)
        {
            _battleFsm = battleFsm;
            _speaker = speaker;
            _aimLayer = aimLayer;
            _selfObject = selfObject;
        }

        public async void Enter()
        {
            _speaker.SpeakDeath();
            _aimLayer.weight = Constants.Animation.IK.Disable;
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