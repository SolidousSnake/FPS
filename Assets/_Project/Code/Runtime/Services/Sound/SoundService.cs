using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using _Project.Code.Runtime.Core.Utils;

namespace _Project.Code.Runtime.Services.Sound
{
    public class SoundService
    {
        [Inject] private readonly AudioMixerGroup _audioMixerGroup;
        [Inject] private readonly AudioSource _musicAudioSource;

        public bool IsMusicMuted { get; private set; }
        public bool IsSfxMuted { get; private set; }

        public void MuteMusic()
        {
            IsMusicMuted = !IsMusicMuted;
            SetMusicVolume(IsMusicMuted ? Constants.Audio.MuteValue : 0f);
        }

        public void MuteSfx()
        {
            IsSfxMuted = !IsSfxMuted;
            SetSfxVolume(IsSfxMuted ? Constants.Audio.MuteValue : 0f);
        }

        public void SetMusicVolume(float volume) =>
            _audioMixerGroup.audioMixer.SetFloat(Constants.Audio.Music, Mathf.Log10(volume) * Constants.Audio.MaxValue);

        public void SetSfxVolume(float volume) =>
            _audioMixerGroup.audioMixer.SetFloat(Constants.Audio.Sfx, Mathf.Log10(volume) * Constants.Audio.MaxValue);
    }
}