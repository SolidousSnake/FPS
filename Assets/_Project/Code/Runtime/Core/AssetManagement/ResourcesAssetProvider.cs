using UnityEngine;

namespace _Project.Code.Runtime.Core.AssetManagement
{
    public class ResourcesAssetProvider : IAssetProvider
    {
        public T Load<T>(string path) where T : Object => Resources.Load<T>(path);
        
        public T[] LoadAll<T>(string path) where T : Object => Resources.LoadAll<T>(path);
    }
}