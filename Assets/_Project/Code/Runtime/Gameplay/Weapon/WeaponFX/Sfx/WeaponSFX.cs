using _Project.Code.Runtime.Config.Weapon.Sfx;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.WeaponFX.Sfx
{
    public class WeaponSfx
    {
        private readonly WeaponSfxConfig _config;
        protected readonly AudioSource FireSource;
        protected readonly AudioSource TailSource;

        private bool _isPlaying;

        public WeaponSfx(AudioSource fireSource, AudioSource tailSource, WeaponSfxConfig config)
        {
            FireSource = fireSource;
            TailSource = tailSource;
            _config = config;
        }

        public void PlayFire()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;
            FireSource.clip = GetRandomFireClip(_config.FireClip);
            TailSource.PlayOneShot(GetRandomFireClip(_config.TailClip));
            FireSource.loop = true;
            FireSource.Play();
        }

        public void StopFire()
        {
            FireSource.Stop();
            FireSource.loop = false;
            _isPlaying = false;
        }
        
        public void PlayReload(bool empty) => 
            FireSource.PlayOneShot(empty ? _config.ReloadEmptyClip : _config.ReloadClip);

        private AudioClip GetRandomFireClip(AudioClip[] clipArray) => clipArray[Random.Range(0, clipArray.Length)];
    }
}