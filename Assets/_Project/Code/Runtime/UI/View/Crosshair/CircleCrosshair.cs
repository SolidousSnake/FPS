using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Runtime.UI.View.Crosshair
{
    public sealed class CircleCrosshair : CrosshairView
    {
        [SerializeField] private RectTransform _circle;

        private Tween _currentTween;
        
        public override void SetAmount(float value, float duration)
        {
            _currentTween?.Kill();
            _currentTween = _circle.DOScale(new Vector3(value * _offset, value * _offset), duration)
                .SetLink(gameObject);
        }
    }
}