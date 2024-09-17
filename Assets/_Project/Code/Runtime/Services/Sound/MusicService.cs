using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Services.Sound
{
    public class MusicService : ITickable, IDisposable
    {
        [Inject] private readonly AudioSource _musicSource;

        private readonly Queue<AudioClip> _clipQueue = new(3);

        public void Tick()
        {
            if (!_musicSource.isPlaying && _clipQueue.Count > 0) 
                PlayNextClip();
        }

        public void PlayImmediately(AudioClip clip)
        {
            Stop();
            Enqueue(clip);
        }

        public void Enqueue(AudioClip clip)
        {
            _musicSource.loop = false;
            _clipQueue.Enqueue(clip);
            PlayNextClip();
        }

        private void PlayNextClip()
        {
            if (_clipQueue.Count == 0 || _musicSource.isPlaying)
                return;

            var clip = _clipQueue.Dequeue();
            _musicSource.clip = clip;
            _musicSource.loop = _clipQueue.Count == 0;

            _musicSource.Play();
        }

        public void Stop()
        {
            _musicSource.Stop();
            _musicSource.clip = null;
        }

        public void Resume() => _musicSource.UnPause();

        public void Pause() => _musicSource.Pause();

        public void Dispose() => Stop();
    }
}