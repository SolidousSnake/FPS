using System;
using _Project.Code.Runtime.Services.Time;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.UI.Label
{
    public class TimeLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private string _format = "m\\:ss";

        [Inject] private Stopwatch _stopwatch;

        public void OnEnable() => _stopwatch.Ticked += SetValue;
        public void OnDisable() => _stopwatch.Ticked -= SetValue;
        
        private void SetValue(TimeSpan value) => _label.text = value.ToString(@_format);
    }
}