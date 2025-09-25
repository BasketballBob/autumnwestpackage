using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class ParticleSystemExtensions
    {
        /// <summary>
        /// Stops the particle system and waits until it no longer has live particles
        /// </summary>
        /// <returns></returns>
        public static IEnumerator StopAndWait(this ParticleSystem ps)
        {
            ps.Stop();
            while (ps.IsAlive()) yield return null;
        }
    }
}
