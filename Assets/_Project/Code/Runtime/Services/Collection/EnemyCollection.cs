using System.Linq;
using System.Collections.Generic;
using _Project.Code.Runtime.AI.States;
using _Project.Code.Runtime.Unit.Enemy.Install;
using UniRx;

namespace _Project.Code.Runtime.Services.Collection
{
    public class EnemyCollection
    {
        private readonly List<EnemyFacade> _enemyList;

        public EnemyCollection()
        {
            _enemyList = new List<EnemyFacade>();
            PlayerSighted = new ReactiveProperty<bool>();
        }

        public readonly ReactiveProperty<bool> PlayerSighted;
        
        public void Register(EnemyFacade enemyFacade) => _enemyList.Add(enemyFacade);
        public void Unregister(EnemyFacade enemyFacade) => _enemyList.Remove(enemyFacade);

        public void SetIdle()
        {
            foreach (var enemy in _enemyList)
                enemy.Fsm.EnterDefaultState();
        }

        public void SetSearch()
        {
            foreach (var enemy in _enemyList)
                enemy.Fsm.Enter<SearchState>();
        }

        public void SetChase()
        {
            foreach (var enemy in _enemyList)
                enemy.Fsm.Enter<ChaseTargetState>();
        }

        public void ReportPlayerSighted() => PlayerSighted.Value = true;

        public void ReportPlayerLost()
        {
            if (EachEnemyLostTarget())
                PlayerSighted.Value = false;
        }

        private bool EachEnemyLostTarget() => _enemyList.All(enemy => !enemy.CanSeePlayer || enemy.Fsm.ActiveState is DeathState);
    }
}