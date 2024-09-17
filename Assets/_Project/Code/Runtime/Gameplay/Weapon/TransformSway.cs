using UnityEngine;

namespace _Project.Code.Runtime.Weapon
{
    public class TransformSway : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] private Vector2 _force = Vector2.one;
        [SerializeField] private float _multiplier;
        [SerializeField] private bool _inverseX;
        [SerializeField] private bool _inverseY;

        [Header("Clamp")] 
        [SerializeField] private Vector2 _minMaxX;
        [SerializeField] private Vector2 _minMaxY;
        
        private const string MouseX = "Mouse X";
        private const string MouseY = "Mouse Y";
        
        private float _additionalX;
        private float _additionalY;

        private float _mouseX, _mouseY;
        private float _velocityY;

        private void Update()
        {
            PerformTransformSway();
        }

        private void PerformTransformSway()
        {
            var deltaTime = Time.deltaTime;
            var inverseSwayX = _inverseX ? -1f : 1f;
            var inverseSwayY = _inverseY ? -1f : 1f;

            _mouseX = Input.GetAxis(MouseX) * inverseSwayX;
            _mouseY = Input.GetAxis(MouseY) * inverseSwayY;
            
            var currentX = _mouseY * _force.y;
            var currentY = _mouseX * _force.x;

            var endEulerAngleX = Mathf.Clamp(currentX + _additionalX, _minMaxX.x, _minMaxX.y);
            var endEulerAngleY = Mathf.Clamp(currentY + _additionalY, _minMaxY.x, _minMaxY.y);

            var moment = deltaTime * _multiplier;
            var localEulerAngles = transform.localEulerAngles;
            
            localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, endEulerAngleX, moment);
            localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, endEulerAngleY, moment);
            localEulerAngles.z = 0f;

            transform.localEulerAngles = localEulerAngles;
        }
    }
}