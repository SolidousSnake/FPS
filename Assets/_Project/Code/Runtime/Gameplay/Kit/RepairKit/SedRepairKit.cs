using _Project.Code.Runtime.Unit.Armor;
using _Project.Code.Runtime.Unit.Health;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.Kit.RepairKit
{
    public class SedRepairKit : MonoBehaviour
    {
        [SerializeField] private Health _sedHealth;
        [SerializeField] private Armor _sedArmor;

        private void Update()
        {
            if(Input.GetKey(KeyCode.V))
                FullRepair();
        }

        public async void FullRepair()
        {
            _sedHealth.ResetHealth();
            await UniTask.WaitForSeconds(.2f);
            _sedArmor.ResetArmor();
        }
    }
}