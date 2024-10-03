using System;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Services.Sound
{
    public class MusicService : IDisposable
    {
        [Inject] private readonly AudioSource _musicSource;
        
        private float _syncTime;

        public void PlayImmediately(AudioClip clip)
        {
            if (_musicSource.isPlaying)
                return;

            _musicSource.clip = clip;
            _musicSource.Play();
            
            if (_syncTime > 0 && _syncTime < _musicSource.clip.length)
                _musicSource.time = _syncTime;
            else
                _syncTime = 0f;
        }

        public void Stop()
        {
            _syncTime = _musicSource.time;
            _musicSource.Stop();
            _musicSource.clip = null;
        }

        public void StopAndReset()
        {
            Stop();
            _syncTime = 0;
        }

        public void Resume() => _musicSource.UnPause();

        public void Pause() => _musicSource.Pause();

        public void Dispose() => Stop();
    }
}