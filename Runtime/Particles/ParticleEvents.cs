using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Sirenix.Utilities;

namespace AWP
{
    public class ParticleEvents : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        private ParticleSystem.Particle[] _currentParticles;
        private Dictionary<uint, ParticleSystem.Particle> _trackedParticles = new Dictionary<uint, ParticleSystem.Particle>();

        private void Update()
        {
            if (!_particleSystem.IsAlive()) return;

            _currentParticles = new ParticleSystem.Particle[_particleSystem.particleCount];
            _particleSystem.GetParticles(_currentParticles);

            // Check for new particles
            _currentParticles.ForEach(x =>
            {
                if (!_trackedParticles.ContainsKey(x.randomSeed))
                {
                    OnParticleEmit(x);
                    _trackedParticles.Add(x.randomSeed, x);
                }
            });

            // Check for dead particles
            var currentParticleDict = _currentParticles.ToDictionary(x => x.randomSeed, x => x);
            for (int i = 0; i < _trackedParticles.Count; i++)
            {
                if (!currentParticleDict.ContainsKey(_trackedParticles.ElementAt(i).Key))
                {
                    OnParticleDeath(_trackedParticles.ElementAt(i).Value);
                    _trackedParticles.Remove(_trackedParticles.ElementAt(i).Key);
                    i--;
                }
            }
        }

        protected virtual void OnParticleEmit(ParticleSystem.Particle particle) { }
        protected virtual void OnParticleDeath(ParticleSystem.Particle particle) { }
    }
}
