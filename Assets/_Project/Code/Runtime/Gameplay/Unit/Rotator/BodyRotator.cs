using UnityEngine;

namespace _Project.Code.Runtime.Unit.Rotator
{
    public class BodyRotator
    {
        private readonly Transform _enemyTransform;
        private readonly float _rotationSpeed;

        public BodyRotator(Transform enemyTransform, float rotationSpeed)
        {
            _enemyTransform = enemyTransform;
            _rotationSpeed = rotationSpeed;
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - _enemyTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        
            _enemyTransform.rotation = Quaternion.Slerp(_enemyTransform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }
}