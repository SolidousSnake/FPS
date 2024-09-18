using _Project.Code.Runtime.Config.AI;
using _Project.Code.Runtime.Config.AI.Quotes;
using _Project.Code.Runtime.Core.AssetManagement;
using _Project.Code.Runtime.Core.Utils;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Speaker
{
    public class EnemySpeaker
    {
        private readonly AudioSource _speechSource;
        private readonly QuotesConfig _quotesConfig;

        public EnemySpeaker(AudioSource speechSource, QuotesConfig quotesConfig)
        {
            _speechSource = speechSource;
            _quotesConfig = quotesConfig;
        }

        public void SpeakChase() => Speak(GetRandomClip(_quotesConfig.ChasingClips));
        public void SpeakDeath() => Speak(GetRandomClip(_quotesConfig.DeathClips));
        public void SpeakSearch() => Speak(GetRandomClip(_quotesConfig.SearchClips));
        public void SpeakCombat() => Speak(GetRandomClip(_quotesConfig.DuringCombatClips));
        public void SpeakTargetSighted() => Speak(GetRandomClip(_quotesConfig.TargetSightedClips));

        private void Speak(AudioClip clip)
        {
            _speechSource.Stop();
            _speechSource.clip = clip;
            _speechSource.Play();
        }

        private AudioClip GetRandomClip(AudioClip[] array) => array[Random.Range(0, array.Length)];
    }
}