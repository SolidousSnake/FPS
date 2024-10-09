using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.States;
using _Project.Code.Runtime.UI.Bar;
using _Project.Code.Runtime.UI.Label;
using _Project.Code.Runtime.UI.View.State;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Factory
{
    public class UIFactory
    {
        [Inject] private readonly IAssetProvider _assetProvider;

        public BossNameLabel CreateBossNameLabel(Transform parent) =>
            CreateUIElement<BossNameLabel>(parent, AssetPath.UIPath.BossLabel);

        public EnemyHealthBar CreateHealthBar(Transform parent) =>
            CreateUIElement<EnemyHealthBar>(parent, AssetPath.UIPath.HealthBar);

        public EnemyArmorBar CreateArmorBar(Transform parent) =>
            CreateUIElement<EnemyArmorBar>(parent, AssetPath.UIPath.ArmorBar);

        public StateView CreateView(Transform parent, InitialState state)
        {
            string path = state switch
            {
                InitialState.Alert => AssetPath.UIPath.AlertView,
                InitialState.Evasion => AssetPath.UIPath.EvasionView,
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"Invalid state: {state}. No corresponding UI view.");
                return null;
            }

            return CreateUIElement<StateView>(parent, path);
        }

        private T CreateUIElement<T>(Transform parent, string path) where T : MonoBehaviour
        {
            var prefab = _assetProvider.Load<T>(path);
            return Object.Instantiate(prefab, parent);
        }
    }
}