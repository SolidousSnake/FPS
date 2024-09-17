using _Project.Code.Runtime.Core.Bootstrap;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Camera;
using _Project.Code.Runtime.Services.Sound;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Installer
{
    public sealed class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private Camera _camera;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_musicSource);
            
            Container.Bind<StateFactory>().AsSingle();
            Container.Bind<GameplayStateMachine>().AsSingle();
            Container.Bind<BattleStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<MusicService>().AsSingle();

            Container.Bind<CameraService>().AsSingle().WithArguments(_camera);
            
            Container.BindInterfacesAndSelfTo<GameplaySceneBootstrapper>().AsSingle().NonLazy();
        }
    }
}