using System.Collections.Generic;
using _Project.Code.Runtime.Core.Bootstrap;
using _Project.Code.Runtime.Core.Factory;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Point.SpawnPoint;
using _Project.Code.Runtime.Services.Camera;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Services.Sound;
using _Project.Code.Runtime.UI.Parent;
using _Project.Code.Runtime.Unit.Player;
using UnityEngine.Serialization;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Installer
{
    public sealed class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private Camera _camera;
        [Space(20)]
        [FormerlySerializedAs("_bossBarParent")] [SerializeField] private BossUiParent _bossUiParent;
        [SerializeField] private RadarParent _radarParent;
        [Space(20)]
        [SerializeField] private PlayerFacade _playerFacade;
        [SerializeField] private List<EnemySpawnPoint> _spawnPoints;
        
        public override void InstallBindings()
        {
            BindFactories();
            BindCollections();
            BindStateMachines();

            Container.BindInstance(_playerFacade);
            Container.BindInstance(_musicSource);
            Container.BindInstance(_bossUiParent);
            Container.BindInstance(_radarParent);
            
            Container.Bind<EnemyCollection>().AsSingle();
            Container.Bind<CameraService>().AsSingle().WithArguments(_camera);
            
            Container.BindInterfacesAndSelfTo<MusicService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplaySceneBootstrapper>().AsSingle().NonLazy();
        }

        private void BindCollections()
        {
            Container.Bind<IEnumerable<EnemySpawnPoint>>().FromInstance(_spawnPoints).AsSingle();
        }

        private void BindStateMachines()
        {
            Container.Bind<GameplayStateMachine>().AsSingle();
            Container.Bind<BattleStateMachine>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<StateFactory>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<UIFactory>().AsSingle();
        }
    }
}