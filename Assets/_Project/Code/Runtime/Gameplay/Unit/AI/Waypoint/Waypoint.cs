using UnityEngine;

namespace _Project.Code.Runtime.Unit.AI.Waypoint
{
    public class Waypoint : MonoBehaviour
    {
        [field: SerializeField] public float WaitingTime { get; private set; }
    }
}