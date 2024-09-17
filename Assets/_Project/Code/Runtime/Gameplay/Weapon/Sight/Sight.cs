using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Sight
{
    public class Sight : MonoBehaviour
    {
        [SerializeField] private float _fovZoom;
        [SerializeField] private float _zoomInMultiplayer;
        [SerializeField] private float _zoomOutMultiplayer;
        [SerializeField] private Vector3 _position;

        public float FovZoom => _fovZoom;
        public float ZoomInMultiplayer => _zoomInMultiplayer;
        public float ZoomOutMultiplayer => _zoomOutMultiplayer;
        public Vector3 Position => _position;
    }
}