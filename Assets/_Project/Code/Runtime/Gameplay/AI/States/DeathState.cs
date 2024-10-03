using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Unit.Enemy.Install;
using _Project.Code.Runtime.Unit.Speaker;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Code.Runtime.AI.States
{
    public class DeathState : IState
    {
        private readonly EnemyCollection _enemyCollection;
        private readonly EnemyFacade _selfObject;
        private readonly EnemySpeaker _speaker;
        private readonly Rig _aimLayer;

        public DeathState(EnemyCollection enemyCollection
            , EnemyFacade selfObject
            , EnemySpeaker speaker
            , Rig aimLayer)
        {
            _enemyCollection = enemyCollection;
            _selfObject = selfObject;
            _speaker = speaker;
            _aimLayer = aimLayer;
        }

        public void Enter()
        {
            _aimLayer.weight = Constants.Animation.IK.Disable;

            _speaker.SpeakDeath();
            _enemyCollection.ReportPlayerLost();
            _enemyCollection.Unregister(_selfObject);
            Object.Destroy(_selfObject.gameObject, 5f);
        }

        public void Exit()
        {
        }
    }
}