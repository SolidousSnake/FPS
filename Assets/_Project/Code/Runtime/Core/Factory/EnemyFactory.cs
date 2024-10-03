using System.Collections.Generic;
using _Project.Code.Runtime.Point.Waypoint;
using _Project.Code.Runtime.Unit.Enemy.Install;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Factory
{
    public class EnemyFactory
    {
        [Inject] private IInstantiator _container;

        public void Create(EnemyFacade prefab, Vector3 position, Quaternion rotation, List<Waypoint> waypoints, MovementMode movementMode)
        {
            var instance = _container.InstantiatePrefab(prefab, position, rotation, null);
            instance.GetComponent<EnemyFacade>().Construct(waypoints, movementMode);
        }
        
        public void Create(EnemyFacade prefab, Vector3 position, Quaternion rotation)
        {
            var instance = _container.InstantiatePrefab(prefab, position, rotation, null);
            instance.GetComponent<EnemyFacade>().Construct();
        }
    }
}