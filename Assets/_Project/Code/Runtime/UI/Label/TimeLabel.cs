using TMPro;
using System;
using UnityEngine;

namespace _Project.Code.Runtime.UI.Label
{
    public class TimeLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private string _format = "m\\:ss";

        public void Initialize() => _label.transform.SetAsLastSibling();
        public void SetValue(TimeSpan value) => _label.text = value.ToString(_format);
    }
}