using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace AWP
{
    public class ParticleSFX : ParticleEvents
    {
        [SerializeField]
        private EventReference _emitSFX;
        [SerializeField]
        private EventReference _deathSFX;

        protected override void OnParticleEmit(ParticleSystem.Particle particle)
        {
            AWGameManager.AudioManager.PlayOneShotAttached(_emitSFX, gameObject);
        }

        protected override void OnParticleDeath(ParticleSystem.Particle particle)
        {
            AWGameManager.AudioManager.PlayOneShotAttached(_deathSFX, gameObject);
        }
    }
}
