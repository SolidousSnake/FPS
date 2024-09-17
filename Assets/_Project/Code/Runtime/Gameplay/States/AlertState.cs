using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.Config.Level;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Sound;

namespace _Project.Code.Runtime.States
{
    public class AlertState : IState
    {
        private readonly MusicConfig _musicConfig;
        private readonly MusicService _musicService;
        
        public AlertState(MusicService musicService, ConfigProvider configProvider)
        {
            _musicService = musicService;
            _musicConfig = configProvider.GetSingle<GameplaySceneConfig>().Music;
        }
        
        public void Enter()
        {
            _musicService.PlayImmediately(_musicConfig.AlertIntroClip);
            _musicService.Enqueue(_musicConfig.AlertLoopClip);
        }

        public void Exit()
        {
            _musicService.Stop();
        }
    }
}