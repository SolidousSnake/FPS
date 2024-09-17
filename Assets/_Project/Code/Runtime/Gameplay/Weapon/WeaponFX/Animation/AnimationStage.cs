using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.WeaponFX.Animation
{
    [System.Serializable]
    public class AnimationStage
    {
        public GameObject GameObject;
        public float Duration;
        public Vector3 Position;
        public Vector3 Rotation;
        public Ease Ease;
    }
}