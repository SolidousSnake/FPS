using _Project.Code.Runtime.Core.Bootstrap;
using _Project.Code.Runtime.Core.Factory;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Camera;
using _Project.Code.Runtime.Services.Sound;
using _Project.Code.Runtime.UI.Parent;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Installer
{
    public sealed class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private Camera _camera;
        [SerializeField] private BossBarParent _bossBarParent;
        
        public override void InstallBindings()
        {
            BindFactory();
            
            Container.BindInstance(_musicSource);
            Container.BindInstance(_bossBarParent);
            
            Container.Bind<GameplayStateMachine>().AsSingle();
            Container.Bind<BattleStateMachine>().AsSingle();
            Container.Bind<CameraService>().AsSingle().WithArguments(_camera);
            
            Container.BindInterfacesAndSelfTo<MusicService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplaySceneBootstrapper>().AsSingle().NonLazy();
        }

        private void BindFactory()
        {
            Container.Bind<StateFactory>().AsSingle();
            Container.Bind<UIFactory>().AsSingle();
        }
    }
}