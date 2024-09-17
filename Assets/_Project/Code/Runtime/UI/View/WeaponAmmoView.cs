using UnityEngine;
using TMPro;

namespace _Project.Code.Runtime.UI.View
{
    public sealed class WeaponAmmoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _ammoLabel;

        public void SetAmount(int first, int second)
        {
            _ammoLabel.text = $"{first}/{second}";
        }
    }
}