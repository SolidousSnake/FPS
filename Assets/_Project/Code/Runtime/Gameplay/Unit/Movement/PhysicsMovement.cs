using System;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Movement
{
    public sealed class PhysicsMovement
    {
        private readonly Rigidbody _rigidBody;
        private readonly Transform _body;
        
        private float _speed;

        public PhysicsMovement(Rigidbody rigidBody, Transform body)
        {
            _rigidBody = rigidBody;
            _body = body;
        }

        public void SetSpeed(float speed)
        {
            if (speed < 0)
                throw new Exception($"Speed must be positive. Received: {speed}");

            _speed = speed;
        }

        public void Move(Vector3 value)
        {
            var movementDirection = _body.forward * value.y + _body.right * value.x;

            _rigidBody.velocity = movementDirection * _speed;
            _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _speed);
        }
    }
}