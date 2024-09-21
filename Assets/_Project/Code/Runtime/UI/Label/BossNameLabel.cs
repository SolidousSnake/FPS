using System;
using TMPro;
using UnityEngine;

namespace _Project.Code.Runtime.UI.Label
{
    public class BossNameLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        public void Initialize(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Name can not be null or empty");

            _label.text = value;
            _label.transform.SetAsLastSibling();
        }
    }
}