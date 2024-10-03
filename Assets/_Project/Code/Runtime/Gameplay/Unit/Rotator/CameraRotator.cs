using _Project.Code.Runtime.Data.Settings;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Rotator
{
    public sealed class CameraRotator
    {
        private readonly Transform _camera;
        private readonly MouseSettings _mouseSettings;
        private readonly Transform _body;
        
        private float _xRotation;
        
        public CameraRotator(Transform camera, MouseSettings mouseSettings, Transform body)
        {
            _camera = camera;
            _mouseSettings = mouseSettings;
            _body = body;
        }
        
        public void SetRotation(Vector2 value)
        {
            _xRotation -= (value.y * Time.deltaTime) * _mouseSettings.Sensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -_mouseSettings.Border, _mouseSettings.Border);
        
            _camera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _body.Rotate(Vector3.up * (value.x * Time.deltaTime) * _mouseSettings.Sensitivity);
        }
    }
}