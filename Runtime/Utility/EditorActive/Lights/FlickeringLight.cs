using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [ExecuteInEditMode]
    public class FlickeringLight : LightAnimator
    {
        [SerializeField]
        private float _minIntensity = 5;
        [SerializeField]
        private float _maxIntensity = 10;
        [SerializeField] 
        private float _perlinNoiseScale = 1;

        protected override float Duration => base.Duration * _perlinNoiseScale;
        
        protected override void OnDeltaChange(float delta)
        {
            _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, AWUnity.PerlinNoise1D(delta, _perlinNoiseScale)) * EnabledDelta;
        }
    }
}
