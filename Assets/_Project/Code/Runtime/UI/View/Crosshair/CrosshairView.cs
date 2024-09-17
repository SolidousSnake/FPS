using UnityEngine;

namespace _Project.Code.Runtime.UI.View.Crosshair
{
    public abstract class CrosshairView : MonoBehaviour
    {
        [SerializeField] protected float _offset;

        public void SetVisibility(bool value) => gameObject.SetActive(value);
        
        public abstract void SetAmount(float value, float duration);
    }
}