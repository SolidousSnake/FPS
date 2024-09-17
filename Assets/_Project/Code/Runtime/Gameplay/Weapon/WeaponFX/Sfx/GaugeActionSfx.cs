using _Project.Code.Runtime.Config.Weapon.Sfx;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.WeaponFX.Sfx
{
    public class GaugeActionSfx : WeaponSfx
    {
        private readonly GaugeActionSfxConfig _config;
        
        public GaugeActionSfx(AudioSource fireSource, AudioSource tailSource, WeaponSfxConfig config) : base(fireSource, tailSource, config)
        {
            _config = config as GaugeActionSfxConfig;
        }

        public void PlayGaugeAction() => FireSource.PlayOneShot(_config.GaugeAction);
    }
}