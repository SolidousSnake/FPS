using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.UI.Bar;
using _Project.Code.Runtime.UI.Label;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Factory
{
    public class UIFactory
    {
        [Inject] private readonly IAssetProvider _assetProvider;
        
        public BossNameLabel CreateBossNameLabel(Transform parent)
        {
            var prefab = _assetProvider.Load<BossNameLabel>(AssetPath.UIPath.BossLabel);
            Debug.Log(prefab);
            Debug.Log(parent);
            var instance = Object.Instantiate(prefab, parent);
            return instance;
        }

        public EnemyHealthBar CreateHealthBar(Transform parent)
        {
            var prefab = _assetProvider.Load<EnemyHealthBar>(AssetPath.UIPath.HealthBar);
            var instance = Object.Instantiate(prefab, parent);
            return instance;
        }
        
        public EnemyArmorBar CreateArmorBar(Transform parent)
        {
            var prefab = _assetProvider.Load<EnemyArmorBar>(AssetPath.UIPath.ArmorBar);
            var instance = Object.Instantiate(prefab, parent);
            return instance;
        }
    }
}