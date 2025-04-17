using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class PerlinNoiseAnimator : EditorActiveAnimator
    {
        [SerializeField] 
        private float _perlinNoiseScale = 1;

        protected override float Duration => base.Duration * _perlinNoiseScale;

        protected float GetPerlinNoise1D(float delta)
        {
            return Mathf.PerlinNoise1D(delta * _perlinNoiseScale);
        }

        // protected float GetPerlinNoise2D(float delta, float yPos)
        // {
        //     delta *= _perlinNoiseScale;
        //     return Mathf.PerlinNoise(yPos, delta);
        // }

        protected Vector3 GetPerlinNoiseVector3(float delta)
        {
            return new Vector3(GetPerlinNoise1D(delta),
                GetPerlinNoise1D(delta + .25f), GetPerlinNoise1D(delta + .25f));
        }
    }
}
