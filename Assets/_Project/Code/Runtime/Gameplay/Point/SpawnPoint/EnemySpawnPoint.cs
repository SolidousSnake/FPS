using System.Collections.Generic;
using _Project.Code.Runtime.Core.Factory;
using _Project.Code.Runtime.Point.Waypoint;
using _Project.Code.Runtime.Unit.Enemy.Install;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Point.SpawnPoint
{
    public class EnemySpawnPoint : SpawnPoint
    {
        [SerializeField] [AssetsOnly]  private EnemyFacade _prefab;
        [SerializeField] private bool _spawnPatrol;
        
        [ShowIf(nameof(_spawnPatrol))] [SerializeField]
        private List<Waypoint.Waypoint> _waypoints;

        [ShowIf(nameof(_spawnPatrol))] [SerializeField]
        private MovementMode _movementMode;
        
        [Inject] private readonly EnemyFactory _enemyFactory;

        public void Spawn() => _enemyFactory.Create(_prefab, transform.position, transform.rotation, _waypoints, _movementMode);
    }
}