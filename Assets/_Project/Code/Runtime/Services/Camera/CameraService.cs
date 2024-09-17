using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Runtime.Services.Camera
{
    public sealed class CameraService
    {
        private readonly UnityEngine.Camera _camera;
        private readonly float _originalFov;
        
        public CameraService(UnityEngine.Camera camera)
        {
            _camera = camera;
            _originalFov = camera.fieldOfView;
        }

        public Transform CameraTransform => _camera.transform;
        
        public void ZoomIn(float fov, float duration) => _camera.DOFieldOfView(fov, duration);
        public void ZoomOut(float duration) => _camera.DOFieldOfView(_originalFov, duration);
    }
}