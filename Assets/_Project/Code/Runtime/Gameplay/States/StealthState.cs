using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Sound;

namespace _Project.Code.Runtime.States
{
    public class StealthState : IState
    {
        private readonly MusicConfig _musicConfig;
        private readonly MusicService _musicService;

        public StealthState(MusicService musicService, ConfigProvider configProvider)
        {
            _musicService = musicService;
            _musicConfig = configProvider.GetSingle<GameplaySceneConfig>().Music;
        }

        public void Enter()
        {
            _musicService.PlayImmediately(_musicConfig.IdleLoopClip);
        }

        public void Exit()
        {
            _musicService.Stop();
        }
    }
}