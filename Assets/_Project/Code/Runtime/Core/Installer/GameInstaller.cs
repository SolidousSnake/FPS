using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.Services.Sound;
using UnityEngine;
using Zenject;

public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private InputService _inputService;
    
    public override void InstallBindings()
    {
        BindServices();
        BindProviders();
    }

    private void BindProviders()
    {
        Container.Bind<ConfigProvider>().AsSingle();
        Container.Bind<IAssetProvider>().To<ResourcesAssetProvider>().AsSingle();
    }

    private void BindServices()
    {
        Container.Bind<SoundService>().AsSingle();
        // Container.BindInterfacesAndSelfTo<MusicService>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputService>().FromComponentInNewPrefab(_inputService).AsSingle();
    }
}