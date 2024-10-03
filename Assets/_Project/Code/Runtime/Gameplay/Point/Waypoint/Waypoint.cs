using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Point.Waypoint
{
    public class Waypoint : MonoBehaviour
    {
        [field: SerializeField] public float WaitingTime { get; private set; }

        [Title("Debug")] [SerializeField] private Color _color;
        [SerializeField] private float _radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}