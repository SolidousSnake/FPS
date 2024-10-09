using UnityEngine;

namespace _Project.Code.Runtime.UI.View.State
{
    public class StateView : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}