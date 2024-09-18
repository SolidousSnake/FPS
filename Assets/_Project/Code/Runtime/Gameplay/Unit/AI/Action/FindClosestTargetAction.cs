using _Project.Code.Runtime.Unit.AI.Sensor;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.AI.Action
{
    public class FindClosestTargetAction
    {
        private readonly VisionSensor _visionSensor;
        private Transform _closestTarget;

        public FindClosestTargetAction(VisionSensor visionSensor)
        {
            _visionSensor = visionSensor;
        }

        public Transform GetClosestTarget()
        {
            var visibleTargets = _visionSensor.VisibleTargets;

            if (visibleTargets.Count == 0)
            {
                _closestTarget = null;
                return null; 
            }

            float minDistance = float.MaxValue;

            foreach (var target in visibleTargets)
            {
                float distance = Vector3.Distance(_visionSensor.transform.position, target.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    _closestTarget = target.transform;
                }
            }
            return _closestTarget; 
        }
    }
}