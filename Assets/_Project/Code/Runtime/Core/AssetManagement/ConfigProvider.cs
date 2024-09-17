using System;
using System.Collections.Generic;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Code.Runtime.Core.AssetManagement
{
    public class ConfigProvider
    {
        [Inject] private IAssetProvider _assetProvider;
        private readonly Dictionary<Type, Object> _singleConfigs = new();
        
        public void LoadSingle<T>(string path) where T : Object => _singleConfigs[typeof(T)] = _assetProvider.Load<T>(path);
        
        public T GetSingle<T>() where T : Object => _singleConfigs[typeof(T)] as T;
        public T GetSingleImmediately<T>(string path) where T : Object => _assetProvider.Load<T>(path);
    }
}