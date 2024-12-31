using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class ParticlePool : ObjectPool<ParticleSystem>
    {
        public void Play() => PullObject().Play();

        protected override bool ObjectIsActive(ParticleSystem obj)
        {
            return obj.isPlaying;
        }
    }
}
