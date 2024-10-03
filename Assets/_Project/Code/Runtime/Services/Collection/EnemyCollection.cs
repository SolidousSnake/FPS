using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Runtime.AI.States;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.States;
using _Project.Code.Runtime.Unit.Enemy.Install;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Services.Collection
{
    public class EnemyCollection
    {
        private readonly List<EnemyFacade> _enemyList;

        public EnemyCollection()
        {
            _enemyList = new List<EnemyFacade>();
        }

        public event Action PlayerSighted;
        public event Action PlayerLost;

        public void Register(EnemyFacade enemyFacade) => _enemyList.Add(enemyFacade);
        public void Unregister(EnemyFacade enemyFacade) => _enemyList.Remove(enemyFacade);

        public void SetIdle()
        {
            foreach (var enemy in _enemyList)
                enemy.Fsm.EnterDefaultState();
        }

        public void SetChase()
        {
            if (EachEnemyChasesTarget())
                return;

            foreach (var enemy in _enemyList)
                enemy.Fsm.Enter<ChaseTargetState>();
        }

        public void ReportPlayerLost()
        {
            Debug.Log(EachEnemyLostTarget());
            if (EachEnemyLostTarget())
                PlayerLost?.Invoke();
        }

        public void ReportPlayerSighted() => PlayerSighted?.Invoke();

        private bool EachEnemyLostTarget() => _enemyList.All(enemy => !enemy.CanSeePlayer);
        private bool EachEnemyChasesTarget() => _enemyList.All(enemy => enemy.Fsm.ActiveState is ChaseTargetState);
    }
}