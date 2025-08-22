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
            AWGameManager.AudioManager.PlayOneShot(_emitSFX);
        }

        protected override void OnParticleDeath(ParticleSystem.Particle particle)
        {
            AWGameManager.AudioManager.PlayOneShot(_deathSFX);
        }
    }
}
